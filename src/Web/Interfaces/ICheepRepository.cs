using Web.Entities;

namespace Web.Interfaces;

public interface ICheepRepository
{
    public Task CreateCheep(Cheep newCheep);
    // Potentially make this return a task
    public List<Cheep> ReadCheeps(string author, int page);
    // Potentially make this return a task
    public List<Cheep> ReadCheeps(int page);

    public Task UpdateCheep(Cheep alteredCheep);

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page);
}
