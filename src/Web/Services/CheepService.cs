using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class CheepService(CheepDbContext context, ICheepRepository cheeprepository) : ICheepService
{
    private readonly CheepDbContext _context = context;
    private readonly ICheepRepository _cheeprepository = cheeprepository;

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
        return _cheeprepository.SearchCheeps(searchQuery, page);
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
        return _cheeprepository.ReadCheeps(page);
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
    {
        return _cheeprepository.ReadCheeps(author, page);
    }

    public void StoreCheep(Cheep cheep)
    {
        _cheeprepository.CreateCheep(cheep);
    }
}
