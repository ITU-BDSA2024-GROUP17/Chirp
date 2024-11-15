using Chirp.Core.Interfaces;
using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Services;

public class CheepService(ICheepRepository cheepRepo) : ICheepRepository
{
    private readonly ICheepRepository _cheepRepo = cheepRepo;

    public Task<Cheep?> GetCheep(int id)
    {
        return _cheepRepo.GetCheep(id);
    }

    /// <summary>
    /// Edits a cheep in the database with the same cheep id and stores the new edit as a revision
    /// </summary>
    /// <param name="cheep">The new</param>
    /// <returns></returns>

    public Task<Cheep> UpdateCheep(Cheep cheep)
    {
        return _cheepRepo.UpdateCheep(cheep);
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

    public Task<Cheep> LikeCheep(int cheepId, string authorId)
    {
        return _cheepRepo.LikeCheep(cheepId, authorId);
    }

    public Task<Cheep> UnlikeCheep(int cheepId, string authorId)
    {
        return _cheepRepo.UnlikeCheep(cheepId, authorId);
    }
}
