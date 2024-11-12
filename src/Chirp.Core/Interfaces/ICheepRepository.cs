using Chirp.Core.DTOs;
using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface ICheepRepository
{
    public Task<Cheep> CreateCheep(Cheep cheep);
    public Task<Cheep?> GetCheep(int id);
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<Cheep> UpdateCheep(Cheep cheep);
    public Task<List<Cheep>> SearchCheeps(string searchQuery, int page);
    public Task<Cheep> LikeCheep(int cheepId, string authorId);
    public Task<Cheep> UnlikeCheep(int cheepId, string authorId);
}
