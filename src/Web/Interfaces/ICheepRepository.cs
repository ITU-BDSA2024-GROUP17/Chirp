using Web.Entities;

namespace Web.Interfaces;

public interface ICheepRepository
{
    public Task CreateCheep(Cheep newCheep);

    public Task<List<Cheep>> ReadCheeps(string author, int page);

    public Task<List<Cheep>> ReadCheeps(int page);

    public Task UpdateCheep(Cheep alteredCheep);
}
