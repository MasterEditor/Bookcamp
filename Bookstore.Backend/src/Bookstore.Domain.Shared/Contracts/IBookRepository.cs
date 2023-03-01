using Bookstore.Domain.Aggregates.BookAggregate;
using MongoDB.Bson;

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
        Task UpdateCommentsByUserId(string id, string imageUrl);
        Task DeleteCommentAsync(string id);
        Task AddRating(Rating rating);
        Task<float> GetRateByAllRates(string bookId);
        Task UpdateBookRate(string bookId, float rate);
        Task<Rating> GetRating(string userId, string bookId);
        Task UpdateRating(string userId, string bookId, int rate);
        Task<Comment> GetComment(string userId, string bookId);
        Task<List<Comment>> GetAllCommentsByBook(string bookId, int? amount);
        Task<List<Book>> GetAllBooksByIds(List<string> bookIds);
        long GetAllDocumentsCount();
        Task DeleteCommentsAndRatingsByUserId(string id);
        Task DeleteCommentsAndRatingsByBookId(string id);
    }
}
