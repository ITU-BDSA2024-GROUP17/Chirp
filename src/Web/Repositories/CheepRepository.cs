using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;
using web.DTOs;

namespace Web.Repositories;

public class CheepRepository(CheepDbContext context) : ICheepRepository
{
    private readonly CheepDbContext _context = context;
    private readonly int _pageSize = 32;

    public Task CreateCheep(CreateCheepDto cheep, Author author)
    {
        _context.Cheeps.Add(new Cheep()
        {
            AuthorId = author.Id,
            Message = cheep.Message,
            TimeStamp = DateTime.Now,
            Author = author
        });

        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public List<Cheep> ReadCheeps(string author, int page)
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

    public List<Cheep> ReadCheeps(int page)
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.TimeStamp)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return cheeps;
    }


    public Task UpdateCheep(Cheep alteredCheep)
    {
        _context.Cheeps.Update(alteredCheep);
        _context.SaveChanges();
        return Task.CompletedTask;
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
}
