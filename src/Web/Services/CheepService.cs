using Web.Interfaces;
using SimpleDB;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Web.Entities;

namespace Web.Services;

public class CheepService(CheepDbContext context) : ICheepService
{
    private readonly CheepDbContext _context = context;

    public List<Cheep> GetCheeps(int page)
    {
        var cheeps = _context.Cheeps.Select(cheep => cheep).ToList();


        return cheeps;
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
    {
        var cheeps = _context.Authors.Where(_author => _author.Name == author).Select(_author => _author.Cheeps).First();

        return (List<Cheep>)cheeps;
    }

    public void StoreCheep(Cheep cheep){
        // TODO: Need a way to store to the db!!
    }
}
