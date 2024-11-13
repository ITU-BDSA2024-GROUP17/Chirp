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
            .Include(c => c.Likes)
            .Select(c => c)
            .OrderByDescending(c => c.TimeStamp)
            .Paginate(page)
            .ToListAsync();

        return cheeps;
    }

    public Task<int> CountCheeps()
    {
        var cheeps = _context.Cheeps.CountAsync();

        return cheeps;
    }

    public Task<Cheep?> GetCheep(int id)
    {
        var cheep = _context.Cheeps
            .Include(c => c.Author)
            .Include(c => c.Likes)
            .FirstOrDefault(c => c.Id == id);

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

    public async Task<Cheep> CreateCheep(Cheep cheep)
    {
        if (cheep.Message.Length > 160) throw new InvalidDataException("Message is too long");
        _context.Cheeps.Add(cheep);
        await _context.SaveChangesAsync();

        return cheep;
    }

    public Task<Cheep> UpdateCheep(Cheep newCheep)
    {
        var cheep = _context.Cheeps.Find(newCheep.Id) ?? throw new InvalidDataException("Cheep does not exist!");

        cheep.Message = newCheep.Message;
        cheep.TimeStamp = newCheep.TimeStamp;

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task<Cheep> LikeCheep(int cheepId, string authorId)
    {
        var cheep = _context.Cheeps.Find(cheepId) ?? throw new InvalidDataException("Cheep does not exist!");
        var author = _context.Authors.Find(authorId) ?? throw new InvalidDataException("Author does not exist!");

        if (cheep.Likes.Contains(author)) throw new Exception("Cheep already liked!");

        cheep.Likes.Add(author);

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task<Cheep> UnlikeCheep(int cheepId, string authorId)
    {
        var cheep = _context.Cheeps.Find(cheepId) ?? throw new InvalidDataException("Cheep does not exist!");
        var author = _context.Authors.Find(authorId) ?? throw new InvalidDataException("Author does not exist!");

        if (!cheep.Likes.Contains(author)) throw new Exception("Cheep not liked!");

        cheep.Likes.Remove(author);

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task DeleteCheep(int cheepId)
    {
        var cheepToDelete = _context.Cheeps.Find(cheepId) ?? throw new Exception("Cheep not found for delete");
        _context.Cheeps.Remove(cheepToDelete);

        return Task.FromResult(_context.SaveChangesAsync());
    }
}
