using CSVDBService.Interfaces;
using SimpleDB;
using SimpleDB.Records;

namespace CSVDBService.Services;

public class CheepService : ICheepService
{
    private readonly CSVDatabase _context = CSVDatabase.Instance;

    public async Task<List<Cheep>> GetCheeps(int page)
    {
        var cheeps = await _context.Read(page);
        return cheeps;
    }

    public async Task<List<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        var cheeps = await _context.Read(author, page);
        return cheeps;
    }
}
