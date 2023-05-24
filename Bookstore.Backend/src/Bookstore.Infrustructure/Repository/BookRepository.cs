using Bookstore.Domain.Aggregates.BookAggregate;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository
{
    public sealed class BookRepository : MongoRepository<Book>, IBookRepository
    {
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<Rating> _ratingCollection;

        public BookRepository(IMongoDbContext context) : base(context)
        {
            _commentCollection = _context.GetCollection<Comment>(GetCollectionName(typeof(Comment)));
            _reviewCollection = _context.GetCollection<Review>(GetCollectionName(typeof(Review)));
            _ratingCollection = _context.GetCollection<Rating>(GetCollectionName(typeof(Rating)));
        }

        public async Task AddRating(Rating rating)
        {
            await _ratingCollection.InsertOneAsync(rating);
        }

        public async Task AddComment(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
        }

        public async Task<List<Review>> FilterReviews(FilterDefinition<Review> filter, string bookId)
        {
            var result = await _reviewCollection
                .Find(filter)
                .ToListAsync();

            return result.Where(x => x.BookId == bookId)
                .ToList();
        }

        public async Task AddReview(Review review)
        {
            await _reviewCollection.InsertOneAsync(review);
        }

        public async Task DeleteCommentAsync(string id)
        {
            var objectId = new ObjectId(id);

            await _reviewCollection.DeleteOneAsync(x => x.Id == objectId);
        }

        public async Task DeleteReviewAsync(string id)
        {
            var objectId = new ObjectId(id);

            await _commentCollection.DeleteOneAsync(x => x.Id == objectId);
        }

        public async Task DeleteCommentsAndRatingsByBookId(string id)
        {
            await _commentCollection.DeleteManyAsync(x => x.BookId == id);
            await _ratingCollection.DeleteManyAsync(x => x.BookId == id);
        }

        public async Task DeleteCommentsAndRatingsByUserId(string id)
        {
            await _commentCollection.DeleteManyAsync(x => x.User.UserId == id);
            await _ratingCollection.DeleteManyAsync(x => x.UserId == id);
        }

        public async Task<List<Book>> GetAllBooksByIds(List<string> bookIds)
        {
            var objectIds = bookIds.Select(x => new ObjectId(x))
                .ToArray();

            var filterDef = new FilterDefinitionBuilder<Book>();
            var filter = filterDef.In(x => x.Id, objectIds);

            return await _collection.Find(filter)
                .ToListAsync();
        }

        public long GetAllDocumentsCount()
        {
            return _collection.CountDocuments(_ => true);
        }

        public async Task<List<Comment>> GetAllCommentsByBook(string bookId, int? amount)
        {
            return await _commentCollection.Find(x => x.BookId == bookId)
                .SortByDescending(x => x.AddedAt)
                .Limit(amount)
                .ToListAsync();
        }

        public async Task<List<Review>> GetAllReviewsByBook(string bookId, int? amount)
        {
            return await _reviewCollection.Find(x => x.BookId == bookId)
                .SortByDescending(x => x.AddedAt)
                .Limit(amount)
                .ToListAsync();
        }

        public async Task<float> GetRateByAllRates(string bookId)
        {
            var rates = await _ratingCollection.Find(x => x.BookId == bookId).ToListAsync();

            if (rates.Count > 0)
            {
                return rates.Sum(x => x.Value) / (float)rates.Count;
            }

            return 0;
        }

        public async Task<Rating> GetRating(string userId, string bookId)
        {
            return await _ratingCollection.Find(x => x.UserId == userId && x.BookId == bookId)
                .FirstOrDefaultAsync();
        }

        public async Task<Comment> GetComment(string userId, string bookId)
        {
            return await _commentCollection.Find(x => x.User.UserId == userId && x.BookId == bookId)
                .FirstOrDefaultAsync();
        }

        public async Task<Review> GetReview(string userId, string bookId)
        {
            return await _reviewCollection.Find(x => x.UserId == userId && x.BookId == bookId)
                .FirstOrDefaultAsync();
        }

        public async Task<Review> GetReview(ObjectId reviewId)
        {
            return await _reviewCollection.Find(x => x.Id == reviewId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateCommentsByUserId(string id, string imageUrl)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.User.UserId, id);
            var update = Builders<Comment>.Update.Set(x => x.User.ImageUrl, imageUrl);

            await _commentCollection.UpdateManyAsync(filter, update);
        }

        public async Task UpdateBookRate(string bookId, float rate)
        {
            var objectId = new ObjectId(bookId);

            var filter = Builders<Book>.Filter.Eq(x => x.Id, objectId);
            var update = Builders<Book>.Update.Set(x => x.Rating, rate);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateFragmentsAsync(ObjectId id, Domain.ValueObjects.Path[] fragments)
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Id, id);
            var update = Builders<Book>.Update.Set(x => x.FragmentPaths, fragments);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateRating(string userId, string bookId, int rate)
        {
            var update = Builders<Rating>.Update.Set(x => x.Value, rate);

            await _ratingCollection.UpdateOneAsync(x => x.UserId == userId && x.BookId == bookId, update);
        }

        public async Task UpdateCoverAsync(ObjectId id, Domain.ValueObjects.Path cover)
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Id, id);
            var update = Builders<Book>.Update.Set(x => x.Cover, cover);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateFragmentAsync(ObjectId id, Domain.ValueObjects.Path fragment)
        {
            var filter = Builders<Book>.Filter.And(
                Builders<Book>.Filter.Eq(x => x.Id, id),
                Builders<Book>.Filter.ElemMatch(x => x.FragmentPaths, x => x.Extension == fragment.Extension));

            var update = Builders<Book>.Update.Set(x => x.FragmentPaths[-1], fragment);

            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteFragmentAsync(ObjectId id, string extention)
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Id, id);
            var update = Builders<Book>.Update.PullFilter(x => x.FragmentPaths, f => f.Extension == extention);

            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task AddFragmentAsync(ObjectId id, Domain.ValueObjects.Path fragment)
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Id, id);
            var update = Builders<Book>.Update.Push(x => x.FragmentPaths, fragment);

            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task UpdateReviewLikes(ObjectId id, List<string> likes)
        {
            var filter = Builders<Review>.Filter.Eq(x => x.Id, id);
            var update = Builders<Review>.Update.Set(x => x.Likes, likes);

            await _reviewCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateReviewDislikes(ObjectId id, List<string> dislikes)
        {
            var filter = Builders<Review>.Filter.Eq(x => x.Id, id);
            var update = Builders<Review>.Update.Set(x => x.Dislikes, dislikes);

            await _reviewCollection.UpdateOneAsync(filter, update);
        }
    }
}
