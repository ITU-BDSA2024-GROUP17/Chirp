using Microsoft.AspNetCore.Mvc;
using SimpleDB;
using SimpleDB.Records;
public interface ICheepService
{
    public Task<List<Cheep>> GetCheeps();
    public Task<List<Cheep>> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly CSVDatabase _context = CSVDatabase.Instance;


    public async Task<List<Cheep>> GetCheeps()
    {
        _context.Store(new Cheep("Alice", "Hello, World!", DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        _context.Store(new Cheep("Bob", "Goodbye, World!", DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        // Get Cheeps from the database
        var cheeps = await _context.Read();
        return cheeps;
    }

    public async Task<List<Cheep>> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        var Cheeps = await _context.Read();
        return Cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
