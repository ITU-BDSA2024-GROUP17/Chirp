using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface IAuthorRepository
{
    public Task<Author> CreateAuthor(Author author);
    public Task<Author?> GetAuthor(string id);
    public Task<Author?> GetAuthorByName(string name);
    public Task<List<Author>> GetAuthors(int page);
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field);
    public Task<List<Cheep>> GetCheeps(string author, int page);
    public Task<int> GetCheepsCount(string author);
    public Task<List<Cheep>> GetCheepsTimeline(string author, int page);
    public Task<int> GetCheepsTimelineCount(string author);
    public Task<List<Cheep>> GetLiked(string author, int page);
    public Task<int> GetLikedCount(string author);
    /// <summary>
    /// Retrieves the cheeps that a given author have commented on
    /// </summary>
    /// <param name="author">Id of the author retrieve the cheeps from.</param>
    /// <param name="page">Page to retrieve the results from.</param>
    /// <returns>Task containing a list of cheeps.</returns>
    public Task<List<Cheep>> GetCheepsCommented(string author, int page);
    /// <summary>
    /// Retrieves the total count of cheeps that a given author have commented on.
    /// </summary>
    /// <param name="author">Id of the author to count the cheeps from.</param>
    /// <returns>Task containing the total count of cheeps.</returns>
    public Task<int> GetCheepsCommentedCount(string author);
    public Task<List<Author>> SearchAuthors(string searchQuery, int page);
    public Task<List<Author>> GetFollowing(string authorId);
    public Task<List<Author>> GetFollowers(string authorId);
    public Task<Author> Follow(string followerId, string followeeId);
    public Task<Author> Unfollow(string followerId, string followeeId);
    public Task<Author> UpdateAuthorPhoneNumber(string id, string phoneNumber);
    public Task<Author> UpdateAuthorAvatar(string id, string avatar);
}
