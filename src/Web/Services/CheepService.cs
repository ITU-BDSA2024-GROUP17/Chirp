using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;
using web.DTOs;
using Web.Repositories;

namespace Web.Services;

public class CheepService(CheepDbContext context, ICheepRepository cheeprepository, IAuthorRepository authorrepository) : ICheepService
{
    private readonly CheepDbContext _context = context;
    private readonly ICheepRepository _cheeprepository = cheeprepository;
    private readonly IAuthorRepository _authorepository = authorrepository;

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        return _authorepository.SearchAuthors(searchQuery, page);
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        return _cheeprepository.SearchCheeps(searchQuery, page);
    }

    public List<Author> GetAuthors(int page)
    {
        return _authorepository.GetAuthors(page);
    }

    public List<Cheep> GetCheeps(int page)
    {
        return _cheeprepository.ReadCheeps(page);
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
    {
        return _cheeprepository.ReadCheeps(author, page);
    }

    public void StoreCheep(CreateCheepDto cheep)
    {
        var author = _authorepository.GetOrCreateAuthor(cheep.Author);
        _cheeprepository.CreateCheep(cheep, author);
    }
}
