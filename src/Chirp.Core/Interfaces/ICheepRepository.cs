using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface ICheepRepository
{
    public List<Cheep> GetCheeps();

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page);

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page);

    public void StoreCheep(Cheep cheep);

}
