using SimpleDB;
using SimpleDB.Records;
public interface ICheepService
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<List<Cheep>> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly CSVDatabase _context = CSVDatabase.Instance;


    public async Task<List<Cheep>> GetCheeps(int page)
    {
        // Get Cheeps from the database
        var cheeps = await _context.Read(page);
        return cheeps;
    }

    public async Task<List<Cheep>> GetCheepsFromAuthor(string author, int page)
    {
        Console.WriteLine(author);
        Console.WriteLine(page);
        return await CSVDatabase.Read(author, page);
    }
}
