using Chirp.Core.Interfaces;
using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Services;

public class CheepService(ICheepRepository cheepRepo) : ICheepRepository
{
    private readonly ICheepRepository _cheepRepo = cheepRepo;

    /// <summary>
    /// Get a specific cheep by id
    /// </summary>
    /// <param name="id">The id of the specified cheep</param>
    /// <returns>Returns the cheep with the specified id</returns>
    public Task<Cheep?> GetCheep(int id)
    {
        return _cheepRepo.GetCheep(id);
    }

    /// <summary>
    /// Edits a cheep in the database with the same cheep id and stores the new edit as a revision
    /// </summary>
    /// <param name="cheep">The new</param>
    /// <returns></returns>

    public Task<Cheep> UpdateCheep(int cheepId, CheepRevision cheepRevision)
    {
        return _cheepRepo.UpdateCheep(cheepId, cheepRevision);
    }

    /// <summary>
    /// Retrieves a paginated list of all Cheeps from the database.
    /// </summary>
    /// <param name="page">The paginated page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of Cheep objects.</returns>
    public Task<List<Cheep>> GetCheeps(int page)
    {
        return _cheepRepo.GetCheeps(page);
    }

    public Task<int> CountCheeps()
    {
        return _cheepRepo.CountCheeps();
    }

    /// <summary>
    /// Searches for Cheeps based on a search query and retrieves a paginated list of results.
    /// </summary>
    /// <param name="searchQuery">The search query </param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of Cheep objects.</returns>
    public Task<List<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        return _cheepRepo.SearchCheeps(searchQuery, page);
    }

    /// <summary>
    /// Stores a new Cheep to the database.
    /// </summary>
    /// <param name="cheep">The Cheep object to store.</param>
    public Task<Cheep> CreateCheep(Cheep cheep)
    {
        return _cheepRepo.CreateCheep(cheep);
    }

    /// <summary>
    /// Stores a like for the specified cheep
    /// </summary>
    /// <param name="cheepId">The cheep that needs to be liked</param>
    /// <param name="authorId">The author that likes the cheep</param>
    /// <returns>Returns the newly liked cheep</returns>
    public Task<Cheep> LikeCheep(int cheepId, string authorId)
    {
        return _cheepRepo.LikeCheep(cheepId, authorId);
    }

    /// <summary>
    /// Deletes the like for a cheep
    /// </summary>
    /// <param name="cheepId">The cheep that needs to be unliked</param>
    /// <param name="authorId">The author that unlikes the cheep</param>
    /// <returns>Returns the newly unliked cheep</returns>
    public Task<Cheep> UnlikeCheep(int cheepId, string authorId)
    {
        return _cheepRepo.UnlikeCheep(cheepId, authorId);
    }


    /// <summary>
    /// Delete the cheep connected with the cheepId
    /// </summary>
    /// <param name="cheepId">The id of the cheep that needs to be deleted</param>
    /// <returns>Returns the task by the database</returns>
    public Task DeleteCheep(int cheepId)
    {
        return _cheepRepo.DeleteCheep(cheepId);
    }
}
