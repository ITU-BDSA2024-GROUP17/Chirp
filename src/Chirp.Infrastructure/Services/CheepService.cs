using Chirp.Core.Interfaces;
using Chirp.Core.Entities;
using Chirp.Core.DTOs;

namespace Chirp.Infrastructure.Services;

public class CheepService(ICheepRepository cheepRepo, IAuthorRepository authorRepo) : ICheepRepository, IAuthorRepository
{
    private readonly ICheepRepository _cheepRepo = cheepRepo;
    private readonly IAuthorRepository _authorRepo = authorRepo;

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
    public Task<Cheep> CreateCheep(CreateCheepDto cheep)
    {
        return _cheepRepo.CreateCheep(cheep);
    }

    public Task<Author?> GetAuthor(int id)
    {
        return _authorRepo.GetAuthor(id);
    }

    /// <summary>
    /// Retrieves all Authors from the database.
    /// </summary>
    /// <returns>A list of Author objects.</returns>
    public Task<List<Author>> GetAuthors(int page)
    {
        return _authorRepo.GetAuthors(page);
    }

    /// <summary>
    /// Retrieves an Author based on a specific field.
    /// <code>
    /// # Example
    /// var authorByName = await GetAuthorByField("John Doe", x => x.Name);
    /// </code>
    /// </summary>
    /// <param name="author">The author identifier.</param>
    /// <param name="field">A function to select the field to search by.</param>
    /// <returns>Task result which contains an Author object</returns>
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field)
    {
        return _authorRepo.GetAuthorByField(author, field);
    }

    /// <summary>
    /// Searches for Authors based on a search query and retrieves a paginated enumerable list of results.
    /// </summary>
    /// <param name="searchQuery">The search query</param>
    /// <param name="page">The paginated page number</param>
    /// <returns>Task result which contains an enumerable of a Author objects</returns>
    public Task<List<Author>> SearchAuthors(string searchQuery, int page)
    {
        return _authorRepo.SearchAuthors(searchQuery, page);
    }

    /// <summary>
    /// Retrieves a paginated list of Cheeps from a specific Author
    /// </summary>
    /// <param name="author">The author</param>
    /// <param name="page">The paginated page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of a Cheep objects.</returns>
    public Task<List<Cheep>> GetCheeps(string author, int page)
    {
        return _authorRepo.GetCheeps(author, page);
    }

    /// <summary>
    /// Creates a new Author.
    /// </summary>
    /// <param name="author">The Author to create.</param>
    public Task CreateAuthor(Author author)
    {
        return _authorRepo.CreateAuthor(author);
    }
}
