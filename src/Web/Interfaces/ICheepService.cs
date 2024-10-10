using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page);
    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page);

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page);


    public List<Cheep> GetCheepsFromAuthor(string author, int page);
    public List<Cheep> GetCheeps();

    public List<Author> GetAuthors();
}
