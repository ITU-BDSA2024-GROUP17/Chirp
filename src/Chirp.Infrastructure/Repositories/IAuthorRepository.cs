using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Interfaces;

public interface IAuthorRepository
{
    /// <summary>
    /// Create a new author in the database.
    /// </summary>
    /// <param name="author">Author object to be inserted into the database.</param>
    /// <returns>Task containing the Author that have been created.</returns>
    public Task<Author> CreateAuthor(Author author);

    /// <summary>
    /// Fetch an author from the database based on the author's id.
    /// </summary>
    /// <param name="id">The id of the author that should be retreived.</param>
    /// <returns>Task containing the Author, if a author with the id was found.</returns>
    public Task<Author?> GetAuthor(string id);

    /// <summary>
    /// Fetch an author from the database based on the author's username.
    /// </summary>
    /// <param name="name">Username of the author that should be retreived.</param>
    /// <returns>Task containing the Author, if a author with the username was found.</returns>
    public Task<Author?> GetAuthorByName(string name);

    /// <summary>
    /// Fetch a list of authors paginated by a page number.
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>Task containing a list of authors found on the requested page.</returns>
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
    /// <returns>Task containing a Author found with the inputed field.</returns>
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field);

    /// <summary>
    /// Retrieves a paginated list of Cheeps from a specific Author
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="author">The author to fetch the cheeps from.</param>
    /// <param name="page">The page to retrieve.</param>
    /// <returns>Task containing a list of cheeps.</returns>
    public Task<List<Cheep>> GetCheeps(string author, int page);

    /// <summary>
    /// Retrieves the total number of cheeps that the author have sent.
    /// </summary>
    /// <param name="author">Id of the author to count the cheeps of.</param>
    /// <returns>Task containing the count of cheeps.</returns>
    public Task<int> GetCheepsCount(string author);

    /// <summary>
    /// Retrieves a list of cheeps belonging to the author.
    /// Along with the cheeps from authors that is followed by the author.
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="author">Id of the author to fetch the list from.</param>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>Task containing the list of cheeps from the retrieved page.</returns>
    public Task<List<Cheep>> GetCheepsTimeline(string author, int page);

    /// <summary>
    /// Retrieves the total count of cheeps on a authors timeline.
    /// </summary>
    /// <param name="author">Id of the author to fetch the count of cheeps from.</param>
    /// <returns>Task containing the total count cheeps on an authors timeline.</returns>
    public Task<int> GetCheepsTimelineCount(string author);

    /// <summary>
    /// Retrieves a list of cheeps that the authors have liked.
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="author">Id of the author</param>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>Task containing the list of cheeps liked by the author.</returns>
    public Task<List<Cheep>> GetLiked(string author, int page);

    /// <summary>
    /// Retrieves the total count of cheeps that an author have liked.
    /// </summary>
    /// <param name="author">Id of the author to count the likes from.</param>
    /// <returns>Task containing the total count of liked cheeps.</returns>
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

    /// <summary>
    /// Retrieves a list of authors based on a search term.
    /// Each page includes 32 entries, unless less is found.
    /// </summary>
    /// <param name="searchQuery">The string that should be used to search in the authors.</param>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>Task containing the authors found based on the search term.</returns>
    public Task<List<Author>> SearchAuthors(string searchQuery, int page);

    /// <summary>
    /// Retrieves a list of authors that is being followed by the provided author.
    /// </summary>
    /// <param name="authorId">Id of the author to fetch the following from.</param>
    /// <returns>Task containing a list of authors.</returns>
    public Task<List<Author>> GetFollowing(string authorId);

    /// <summary>
    /// Retrieves a list of authors that is following the provided author.
    /// </summary>
    /// <param name="authorId">Id of the author to fetch the followers from.</param>
    /// <returns>Task containing a list of authors.</returns>
    public Task<List<Author>> GetFollowers(string authorId);

    /// <summary>
    /// Add a follow to an author.
    /// </summary>
    /// <param name="followerId">Id of the author that is starting to follow.</param>
    /// <param name="followeeId">Id of the author that is starting to being followed.</param>
    /// <returns>Task containing the new author object of the author that is following.</returns>
    public Task<Author> Follow(string followerId, string followeeId);

    /// <summary>
    /// Remove a follow to an author.
    /// </summary>
    /// <param name="followerId">Id of the author that is stopping to follow.</param>
    /// <param name="followeeId">Id of the author that is stopping to being followed.</param>
    /// <returns>Task containing the new author object of the author that is no longer following.</returns>
    public Task<Author> Unfollow(string followerId, string followeeId);

    /// <summary>
    /// Update the phone number of an author.
    /// </summary>
    /// <param name="id">Id of the author to update.</param>
    /// <param name="phoneNumber">New phone number to be updated on the author.</param>
    /// <returns>Task containing the updated author.</returns>
    public Task<Author> UpdateAuthorPhoneNumber(string id, string phoneNumber);

    /// <summary>
    /// Update the avatar of an author.
    /// </summary>
    /// <param name="id">Id of the author to update.</param>
    /// <param name="avatar">New avatar to be updated on the author.</param>
    /// <returns>Task containing the updated avatar.</returns>
    public Task<Author> UpdateAuthorAvatar(string id, string avatar);
}
