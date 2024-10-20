using Chirp.Core.Interfaces;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class CheepRepository(CheepDbContext context) : ICheepRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Cheep>> GetCheeps()
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
            .ToListAsync();

        return cheeps;
    }

    public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        var messages = GetCheeps().Result
            .AsEnumerable()
            .Search(searchQuery, x => x.Message).Paginate(page);

        return Task.FromResult(messages);
    }

    public Task<IEnumerable<Cheep>> GetAllCheeps(int page)
    {
        var cheeps = GetCheeps().Result
            .AsEnumerable()
            .Paginate(page);

        return Task.FromResult(cheeps);
    }

    public Task StoreCheep(Cheep cheep)
    {
        _context.Cheeps.Add(cheep);
        var tempAuth = _context.Authors.Where(_author => _author.Id == cheep.Author.Id).Select(_author => _author).First();
        if (tempAuth == null)
        {
            _context.Authors.Add(cheep.Author);
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}
