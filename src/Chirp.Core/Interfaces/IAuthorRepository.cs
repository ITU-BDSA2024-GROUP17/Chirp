using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface IAuthorRepository
{
    /// <summary>
    /// Create a new author in the database.
    /// </summary>
    /// <param name="author">Author object to be inserted into the database.</param>
    /// <returns>Author that have been created.</returns>
    public Task<Author> CreateAuthor(Author author);
    /// <summary>
    /// Fetch an author from the database based on the author's id.
    /// </summary>
    /// <param name="id">The id of the author that should be retreived.</param>
    /// <returns>Author, if a author with the id was found.</returns>
    public Task<Author?> GetAuthor(string id);
    /// <summary>
    /// Fetch an author from the database based on the author's username.
    /// </summary>
    /// <param name="name">Username of the author that should be retreived.</param>
    /// <returns>Author, if a author with the username was found.</returns>
    public Task<Author?> GetAuthorByName(string name);
    /// <summary>
    /// Fetch a list of authors paginated by a page number.
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>List of authors found on the requested page.</returns>
    public Task<List<Author>> GetAuthors(int page);
    /// <summary>
    /// Retrieves an Author based on a specific field.
    ///
    /// <code>
    /// # Example
    /// var authorByName = await GetAuthorByField("John Doe", x => x.Name);
    /// </code>
    ///
    /// </summary>
    /// <param name="author">The author identifier.</param>
    /// <param name="field">A function to select the field to search by.</param>
    /// <returns>Task result which contains an Author object</returns>
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field);
    /// <summary>
    /// Retrieves a paginated list of Cheeps from a specific Author
    /// </summary>
    /// <param name="author">The author</param>
    /// <param name="page">The paginated page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of a Cheep objects.</returns>
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
