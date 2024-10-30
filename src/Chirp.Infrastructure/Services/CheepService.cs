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
}
