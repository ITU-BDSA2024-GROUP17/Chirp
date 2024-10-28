using Chirp.Core.Interfaces;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class CheepRepository(CheepDbContext context) : ICheepRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Cheep>> GetCheeps(int page)
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderByDescending(c => c.TimeStamp)
            .Paginate(page)
            .ToListAsync();

        return cheeps;
    }

    public Task<Cheep?> GetCheep(int id)
    {
        var cheep = _context.Cheeps.Find(id);

        return Task.FromResult(cheep);
    }

    public Task<List<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        var cheeps = _context.Cheeps
            .Search(searchQuery, x => x.Message)
            .OrderByDescending(c => c.TimeStamp)
            .Paginate(page)
            .ToList();

        return Task.FromResult(cheeps);
    }

    public Task<Cheep> CreateCheep(Cheep cheep)
    {
        _context.Cheeps.Add(cheep);
        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task<Cheep> UpdateCheep(Cheep newCheep)
    {
        var cheep = _context.Cheeps.Find(newCheep.Id) ?? throw new InvalidDataException("Author does not exist!");

        cheep.Message = newCheep.Message;
        cheep.TimeStamp = newCheep.TimeStamp;

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }
}
