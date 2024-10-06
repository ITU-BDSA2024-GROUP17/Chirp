using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public Tuple<HashSet<Author>, HashSet<Cheep>> GetCheeps(string searchQuery);
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsFromAuthor(string author, int page);
}
