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
    public Task<List<Author>> SearchAuthors(string searchQuery, int page);
    public Task<List<Author>> GetFollowing(string authorId);
    public Task<List<Author>> GetFollowers(string authorId);
    public Task<Author> Follow(string followerId, string followeeId);
    public Task<Author> Unfollow(string followerId, string followeeId);
    public Task<Author> UpdateAuthorPhoneNumber(string id, string phoneNumber);
    public Task<Author> UpdateAuthorAvatar(string id, string avatar);
}
