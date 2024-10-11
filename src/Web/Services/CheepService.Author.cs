using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Web.Services;

public class CheepServiceAuthor(CheepDbContext context) : ICheepService.IAuthor
{
    private readonly CheepDbContext _context = context;

    public List<Author> GetAuthors()
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.Name)
            .ToList();
        return authors;
    }

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = GetAuthors()
        .Select(a => a)
        .OrderBy(a => a.Name)
        .AsEnumerable()
        .Search(searchQuery, x => x.Name).Paginate(page);

        return Task.FromResult(authors);
    }


    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        var cheeps = _context.Authors
          .Where(a => a.Name == author)
          .SelectMany(a => a.Cheeps)
          .Include(c => c.Author)
          .OrderBy(c => c.TimeStamp)
          .Paginate(page);

        return Task.FromResult(cheeps);
    }
}
