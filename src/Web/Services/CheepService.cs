using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = _context.Authors
         .Select(a => a)
         .OrderBy(a => a.Name)
         .AsEnumerable()
         .Search(searchQuery, x => x.Name).Paginate(page);

        return Task.FromResult(authors);
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        var messages = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
            .AsEnumerable()
            .Search(searchQuery, x => x.Message).Paginate(page);

        return Task.FromResult(messages);
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
    {
        var cheeps = _context.Authors
            .Where(a => a.Name == author)
            .SelectMany(a => a.Cheeps)
            .Include(c => c.Author)
            .OrderBy(c => c.TimeStamp)
            .Paginate(page).ToList();

        return cheeps;
    }

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page)
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
            .AsEnumerable()
            .Paginate(page);

        return Task.FromResult(cheeps);
    }

    public List<Author> GetAuthors()
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.Name).ToList();
        return authors;
    }

    public List<Cheep> GetCheeps()
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
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

