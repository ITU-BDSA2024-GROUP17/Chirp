using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface IAuthorRepository
{
    public Task<List<Author>> GetAuthors();
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field);
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page);
    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page);
    public Task CreateAuthor(Author author);
}
