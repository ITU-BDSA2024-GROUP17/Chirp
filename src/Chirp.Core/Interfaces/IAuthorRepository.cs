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
    public Task<List<Author>> SearchAuthors(string searchQuery, int page);
}
