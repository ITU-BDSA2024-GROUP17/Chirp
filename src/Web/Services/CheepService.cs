using Web.Interfaces;
using Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    private readonly int _pageSize = 32;

    public Tuple<HashSet<Author>, HashSet<Cheep>> GetCheeps(string searchQuery)
    {
        var authors = _context.Cheeps
            .Include(c => c.Author).Select(c => c.Author).Where(a => a.Name.ToLower().Contains(searchQuery.ToLower())).ToHashSet();
        var messages = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .Where(c => c.Message.ToLower().Contains(searchQuery.ToLower())).ToHashSet();



        //var authors = cheeps.Where(c => c.Author.Name.ToLower().Contains(searchQuery.ToLower())).ToHashSet();
        Console.WriteLine("Count Author: " + authors);
        Console.WriteLine("Count Cheeps: " + messages);
        return Tuple.Create(authors, messages);

    }

    public List<Cheep> GetCheeps(int page)
    {
        var cheeps = _context.Cheeps
            .Include(c => c.Author)
            .Select(c => c)
            .OrderBy(c => c.TimeStamp)
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
            .OrderBy(c => c.TimeStamp)
            .Skip(Math.Max(0, page - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        return cheeps;
    }

    public void StoreCheep(Cheep cheep)
    {
        _context.Cheeps.Add(cheep);
        var tempAuth = _context.Authors.Where(_author => _author.Id == cheep.Author.Id).Select(_author => _author).First();
        if (tempAuth == null)
        {
            _context.Authors.Add(cheep.Author);
        }
        _context.SaveChanges();
    }
}
