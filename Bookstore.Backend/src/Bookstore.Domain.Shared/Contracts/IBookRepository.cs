using Bookstore.Domain.Aggregates.BookAggregate;
using MongoDB.Bson;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IBookRepository : IMongoRepository<Book>
    {
        Task UpdateFragmentsAsync(ObjectId id, ValueObjects.Path[] fragments);
        Task UpdateCoverAsync(ObjectId id, ValueObjects.Path cover);
        Task AddReview(Review review);
        Task UpdateReviewsByUserId(string id, string imageUrl);
        Task DeleteReviewAsync(string id);
        Task AddRating(Rating rating);
        Task<float> GetRateByAllRates(string bookId);
        Task UpdateBookRate(string bookId, float rate);
        Task<Rating> GetRating(string userId, string bookId);
        Task UpdateRating(string userId, string bookId, int rate);
        Task<Review> GetReview(string userId, string bookId);
        Task<List<Review>> GetAllReviewsByBook(string bookId);
        Task<List<Book>> GetAllBooksByIds(List<string> bookIds);
        long GetAllDocumentsCount();
        Task DeleteReviewsAndRatingsByUserId(string id);
        Task DeleteReviewsAndRatingsByBookId(string id);
    }
}
