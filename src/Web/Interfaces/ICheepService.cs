using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public Task<Tuple<IEnumerable<Author>, IEnumerable<Cheep>>> GetCheeps(string searchQuery, int chunkSize);
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsFromAuthor(string author, int page);
}
