using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Repositories;

public class AuthorRepository(CheepDbContext context) : IAuthorRepository
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
}
