using Web.Interfaces;
using Web.Entities;

namespace Web.Services;

public class CheepService(ICheepRepository cheepRepo, IAuthorRepository authorRepo) : ICheepRepository, IAuthorRepository
{
    private readonly ICheepRepository _cheepRepo = cheepRepo;
    private readonly IAuthorRepository _authorRepo = authorRepo;

    public List<Cheep> GetCheeps()
    {
        return _cheepRepo.GetCheeps();
    }

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page)
    {
        return _cheepRepo.GetAllCheeps(page);
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        return _cheepRepo.SearchCheeps(searchQuery, page);
    }

    public void StoreCheep(Cheep cheep)
    {
        _cheepRepo.StoreCheep(cheep);
    }


    public List<Author> GetAuthors()
    {
        return _authorRepo.GetAuthors();
    }

    public Task<Author> GetAuthorByField(string author, Func<Author, string> field)
    {
        return _authorRepo.GetAuthorByField(author, field);
    }

    public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page)
    {
        return _authorRepo.SearchAuthors(searchQuery, page);
    }

    public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        return _authorRepo.GetCheepsFromAuthor(author, page);
    }

    public void createAuthor(Author author)
    {
        _authorRepo.createAuthor(author);
    }
}
