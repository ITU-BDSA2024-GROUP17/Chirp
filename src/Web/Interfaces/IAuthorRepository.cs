using Web.Entities;

namespace Web.Interfaces;

public interface IAuthorRepository
{
    public List<Author> GetAuthors();
    public Task<Author> GetAuthorByField(string author, Func<Author, string> field);
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page);
    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page);
    public void createAuthor(Author author);
}
