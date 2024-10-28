using Chirp.Core.Interfaces;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class AuthorRepository(CheepDbContext context) : IAuthorRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Author>> GetAuthors(int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.UserName)
            .Paginate(page)
            .ToListAsync();

        return authors;
    }

    public Task<Author?> GetAuthor(string id)
    {
        try
        {
            var author = _context.Authors.Find(id);
            return Task.FromResult<Author?>(author);
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult<Author?>(null);
        }
    }

    public Task<Author?> GetAuthorByName(string name)
    {
        try
        {
            var author = _context.Authors.Where(a => a.UserName == name).First();
            return Task.FromResult<Author?>(author);
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult<Author?>(null);
        }
    }

    public Task<List<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .Search(searchQuery, x => x.UserName ?? "")
            .OrderBy(a => a.UserName)
            .Paginate(page)
            .ToList();

        return Task.FromResult(authors);
    }

    public Task<List<Cheep>> GetCheeps(string author, int page)
    {
        var cheeps = _context.Authors
          .Where(a => a.UserName == author)
          .SelectMany(a => a.Cheeps)
          .Include(c => c.Author)
          .OrderByDescending(c => c.TimeStamp)
          .Paginate(page)
          .ToList();

        return Task.FromResult(cheeps);
    }

    public Task<Author> GetAuthorByField(string author, Func<Author, string> field)
    {
        var FoundAuthor = _context.Authors
            .Search(author, field)
            .First();

        return Task.FromResult(FoundAuthor);
    }

    public Task<Author> CreateAuthor(Author author)
    {
        if (_context.Authors.Find(author.Id) != null) throw new InvalidDataException("Author is not avaliable");

        _context.Authors.Add(author);
        _context.SaveChanges();
        return Task.FromResult(author);
    }
}
