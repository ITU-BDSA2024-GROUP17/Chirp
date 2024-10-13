using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;
using web.DTOs;

namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    private readonly int _pageSize = 32;

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = _context.Authors
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
            .OrderByDescending(c => c.TimeStamp)
            .AsEnumerable()
            .Where(c => c.Message.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize);

        return Task.FromResult(messages);
    }

    public List<Author> GetAuthors(int page)
    {
        var authors = _context.Authors
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
            .OrderByDescending(c => c.TimeStamp)
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
            .OrderByDescending(c => c.TimeStamp)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return cheeps;
    }

    public Author GetOrCreateAuthor(string author)
    {
        var query = _context.Authors.Where(_author => _author.Name == author);
        if (!query.Any())
        {
            var authorObj = new Author() { Name = author, Email = "" };
            _context.Authors.Add(authorObj);
            _context.SaveChanges();
            return authorObj;
        }
        return query.First();
    }

    public void StoreCheep(CreateCheepDto cheep)
    {
        var author = GetOrCreateAuthor(cheep.Author);
        _context.Cheeps.Add(new Cheep()
        {
            AuthorId = author.Id,
            Message = cheep.Message,
            TimeStamp = DateTime.Now,
            Author = author
        });

        _context.SaveChanges();
    }
}
