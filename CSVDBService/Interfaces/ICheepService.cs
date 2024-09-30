using SimpleDB.Records;

namespace CSVDBService.Interfaces;

public interface ICheepService
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<List<Cheep>> GetCheepsFromAuthor(string author, int page);
}
