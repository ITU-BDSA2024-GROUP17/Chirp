using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Interfaces;

public interface ICheepRepository
{
    /// <summary>
    /// Add a new cheep to the database.
    /// </summary>
    /// <param name="cheep">Cheep to save in the database.</param>
    /// <returns>Task containing to cheep that was added to the database.</returns>
    public Task<Cheep> CreateCheep(Cheep cheep);

    /// <summary>
    /// Retrieves an cheep based on its id.
    /// </summary>
    /// <param name="id">Id of the cheep to retrieve.</param>
    /// <returns>Task containing the cheep, if it was found based on its id.</returns>
    public Task<Cheep?> GetCheep(int id);

    /// <summary>
    /// Retrieve the cheeps on a specific page.
    /// </summary>
    /// <param name="page">Page to be retrieved.</param>
    /// <returns>Task containing a list of cheeps on the requested page.</returns>
    public Task<List<Cheep>> GetCheeps(int page);

    /// <summary>
    /// Retrieve the total count of cheeps.
    /// </summary>
    /// <returns>Task containing the total count of cheeps.</returns>
    public Task<int> CountCheeps();

    /// <summary>
    /// Update the message on a cheep.
    /// This will create a new revision with the new information.
    /// </summary>
    /// <param name="cheepId">Id of the cheep to update.</param>
    /// <param name="cheepRevision">The new revision to be added to the cheep.</param>
    /// <returns>Task containing the updated cheep.</returns>
    public Task<Cheep> UpdateCheep(int cheepId, CheepRevision cheepRevision);

    /// <summary>
    /// Retrieve a cheeps based on a given search term.
    /// </summary>
    /// <param name="searchQuery">The string to search for in the cheeps.</param>
    /// <param name="page">The page to retrieve.</param>
    /// <returns>Task containing a list of cheeps.</returns>
    public Task<List<Cheep>> SearchCheeps(string searchQuery, int page);

    /// <summary>
    /// Post a comment to an already existing cheep.
    /// </summary>
    /// <param name="CheepToCommentId">Id of the cheep to comment on.</param>
    /// <param name="comment">A cheep to post on an cheep.</param>
    /// <returns>Task</returns>
    public Task PostComment(int CheepToCommentId, Cheep comment);

    /// <summary>
    /// Apply a like on a cheep based from the given author.
    /// </summary>
    /// <param name="cheepId">Id of the cheep to like.</param>
    /// <param name="authorId">Id of the author that liked.</param>
    /// <returns>Task containing the updated cheep.</returns>
    public Task<Cheep> LikeCheep(int cheepId, string authorId);

    /// <summary>
    /// Remove the like from a cheep that was previously given by an author.
    /// </summary>
    /// <param name="cheepId">Id of the cheep to remove the like from.</param>
    /// <param name="authorId">Id of the author that removed the like.</param>
    /// <returns>Task containing the updated cheep.</returns>
    public Task<Cheep> UnlikeCheep(int cheepId, string authorId);

    /// <summary>
    /// Removes a given cheep from the database.
    /// </summary>
    /// <param name="cheepId">Id of the cheep to remove.</param>
    /// <returns>Task</returns>
    public Task DeleteCheep(int cheepId);
}
