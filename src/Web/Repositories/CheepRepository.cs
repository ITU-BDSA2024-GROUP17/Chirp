using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Repositories;

public class CheepRepository(CheepDbContext context) : ICheepRepository
{
    private readonly CheepDbContext _context = context;
    private readonly int _pageSize = 32;

    public Task CreateCheep(Cheep newCheep)
    {
        _context.Cheeps.Add(newCheep);
        var tempAuth = _context.Authors
            .Where(_author => _author.Id == newCheep.Author.Id)
            .Select(_author => _author)
            .First();
        if (tempAuth == null)
        {
            _context.Authors.Add(newCheep.Author);
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public List<Cheep> ReadCheeps(string author, int page)
    {
        var cheeps = _context.Authors
            .Where(a => a.Name == author)
            .SelectMany(a => a.Cheeps)
            .Include(c => c.Author)
            .OrderBy(c => c.TimeStamp)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return cheeps;
    }

    public List<Cheep> ReadCheeps(int page)
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return cheeps;
    }


    public Task UpdateCheep(Cheep alteredCheep)
    {
        _context.Cheeps.Update(alteredCheep);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        var messages = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
            .AsEnumerable()
            .Where(c => c.Message.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize);

        return Task.FromResult(messages);
    }
}
