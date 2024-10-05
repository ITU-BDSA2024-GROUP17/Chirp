using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public List<Cheep> GetCheeps();
    public List<Cheep> GetCheepsFromAuthor(string author);
}
