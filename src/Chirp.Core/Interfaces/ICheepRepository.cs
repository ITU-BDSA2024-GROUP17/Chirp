using Core.Entities;

namespace Core.Interfaces;

public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps();

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page);

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page);

    public Task StoreCheep(Cheep cheep);

}
