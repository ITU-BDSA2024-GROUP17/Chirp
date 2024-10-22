using Chirp.Core.Interfaces;
using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Services;

public class CheepService(ICheepRepository cheepRepo, IAuthorRepository authorRepo) : ICheepRepository, IAuthorRepository
{
    private readonly ICheepRepository _cheepRepo = cheepRepo;
    private readonly IAuthorRepository _authorRepo = authorRepo;

    /// <summary>
    /// Retrieves all Cheeps from the database.
    /// </summary>
    /// <returns>A list of Cheep objects.</returns>
    public Task<List<Cheep>> GetCheeps()
    {
        return _cheepRepo.GetCheeps();
    }

    /// <summary>
    /// Retrieves a paginated list of all Cheeps from the database.
    /// </summary>
    /// <param name="page">The paginated page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of Cheep objects.</returns>
    public Task<IEnumerable<Cheep>> GetAllCheeps(int page)
    {
        return _cheepRepo.GetAllCheeps(page);
    }

    /// <summary>
    /// Searches for Cheeps based on a search query and retrieves a paginated list of results.
    /// </summary>
    /// <param name="searchQuery">The search query </param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of Cheep objects.</returns>
    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        return _cheepRepo.SearchCheeps(searchQuery, page);
    }

    /// <summary>
    /// Stores a new Cheep to the database.
    /// </summary>
    /// <param name="cheep">The Cheep object to store.</param>
    public Task StoreCheep(Cheep cheep)
    {
        return _cheepRepo.StoreCheep(cheep);
    }

    /// <summary>
    /// Retrieves all Authors from the database.
    /// </summary>
    /// <returns>A list of Author objects.</returns>
    public Task<List<Author>> GetAuthors()
    {
        return _authorRepo.GetAuthors();
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
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        return _authorRepo.SearchAuthors(searchQuery, page);
    }

    /// <summary>
    /// Retrieves a paginated list of Cheeps from a specific Author
    /// </summary>
    /// <param name="author">The author</param>
    /// <param name="page">The paginated page number to retrieve.</param>
    /// <returns>Task result which contains an enumerable of a Cheep objects.</returns>
    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        return _authorRepo.GetCheepsFromAuthor(author, page);
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
