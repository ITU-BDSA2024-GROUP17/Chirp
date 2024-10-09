using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    private readonly int _pageSize = 32;

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.Name)
            .AsEnumerable()
            .Where(a => a.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize);


        return Task.FromResult(authors);
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

    public List<Author> GetAuthors(int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.Name)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return authors;
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
