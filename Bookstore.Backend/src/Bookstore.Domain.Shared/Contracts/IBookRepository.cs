using System.Linq.Expressions;
using Bookstore.Domain.Aggregates.BookAggregate;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IBookRepository : IMongoRepository<Book>
    {
        Task UpdateFragmentsAsync(ObjectId id, ValueObjects.Path[] fragments);
        Task UpdateFragmentAsync(ObjectId id, ValueObjects.Path fragment);
        Task DeleteFragmentAsync(ObjectId id, string extention);
        Task AddFragmentAsync(ObjectId id, ValueObjects.Path fragment);
        Task UpdateCoverAsync(ObjectId id, ValueObjects.Path cover);
        Task AddComment(Comment comment);
        Task AddReview(Review review);
        Task UpdateCommentsByUserId(string id, string imageUrl);
        Task DeleteCommentAsync(string id);
        Task DeleteReviewAsync(string id);
        Task AddRating(Rating rating);
        Task<float> GetRateByAllRates(string bookId);
        Task UpdateBookRate(string bookId, float rate);
        Task<Rating> GetRating(string userId, string bookId);
        Task UpdateRating(string userId, string bookId, int rate);
        Task<Comment> GetComment(string userId, string bookId);
        Task<Review> GetReview(string userId, string bookId);
        Task<Review> GetReview(ObjectId reviewId);
        Task<List<Comment>> GetAllCommentsByBook(string bookId, int? amount);
        Task<List<Review>> GetAllReviewsByBook(string bookId, int? amount);
        Task<List<Book>> GetAllBooksByIds(List<string> bookIds);
        long GetAllDocumentsCount();
        Task DeleteCommentsAndRatingsByUserId(string id);
        Task DeleteCommentsAndRatingsByBookId(string id);
        Task UpdateReviewLikes(ObjectId id, List<string> likes);
        Task UpdateReviewDislikes(ObjectId id, List<string> dislikes);
        Task<List<Review>> FilterReviews(FilterDefinition<Review> filter, string bookId);
        Task<List<Review>> FilterReviews(Expression<Func<Review, bool>> filterExpression);
    }
}
