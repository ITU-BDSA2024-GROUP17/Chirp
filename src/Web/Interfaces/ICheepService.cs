using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int chunkSize);
    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int chunkSize);
    public List<Author> GetAuthors(int page);
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsFromAuthor(string author, int page);
    public void StoreCheep(Cheep cheep);
}
