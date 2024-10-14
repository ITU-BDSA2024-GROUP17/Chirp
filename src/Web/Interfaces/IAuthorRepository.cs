using Web.Entities;

namespace Web.Interfaces;

public interface IAuthorRepository
{
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page);
    public List<Author> GetAuthors(int page);
}
