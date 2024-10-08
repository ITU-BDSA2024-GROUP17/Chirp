using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    private readonly int _pageSize = 32;

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int chunkSize)
    {
        var authors = _context.Cheeps
               .Include(c => c.Author)
               .Select(c => c.Author)
               .Where(a => a.Name.ToLower().Contains(searchQuery.ToLower()))
               .AsEnumerable().ToHashSet()
               .Take(chunkSize);

        return Task.FromResult(authors);
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int chunkSize)
    {
        var messages = _context.Cheeps
            .Include(c => c.Author)
            .Where(c => c.Message.ToLower().Contains(searchQuery.ToLower()))
            .AsEnumerable().ToHashSet()
            .Take(chunkSize);

        return Task.FromResult(messages);
    }

    public List<Cheep> GetCheeps(int page)
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

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
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

    public void StoreCheep(Cheep cheep)
    {
        _context.Cheeps.Add(cheep);
        var tempAuth = _context.Authors.Where(_author => _author.Id == cheep.Author.Id).Select(_author => _author).First();
        if (tempAuth == null)
        {
            _context.Authors.Add(cheep.Author);
        }
        _context.SaveChanges();
    }
}
