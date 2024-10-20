using Core.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorRepository(CheepDbContext context) : IAuthorRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Author>> GetAuthors()
    {
        List<Author> authors = [.. _context.Authors
            .Select(a => a)
            .OrderBy(a => a.Name)];
        return Task.FromResult(authors);
    }

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        IEnumerable<Author> authors = GetAuthors().Result
        .Select(a => a)
        .OrderBy(a => a.Name)
        .AsEnumerable()
        .Search(searchQuery, x => x.Name).Paginate(page);

        return Task.FromResult(authors);
    }


    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        IEnumerable<Cheep> cheeps = _context.Authors
          .Where(a => a.Name == author)
          .SelectMany(a => a.Cheeps)
          .Include(c => c.Author)
          .OrderBy(c => c.TimeStamp)
          .Paginate(page);

        return Task.FromResult(cheeps);
    }

    public Task<Author> GetAuthorByField(string author, Func<Author, string> field)
    {
        Author FoundAuthor = GetAuthors().Result.Search(author, field).First();

        return Task.FromResult(FoundAuthor);
    }

    public Task CreateAuthor(Author author)
    {
        if (GetAuthors().Result.Contains(author)) throw new InvalidDataException("Author is not avaliable");


        _context.Authors.Add(author);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

}
