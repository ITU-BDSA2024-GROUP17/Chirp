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
            .OrderBy(a => a.Name)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return authors;
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
}
