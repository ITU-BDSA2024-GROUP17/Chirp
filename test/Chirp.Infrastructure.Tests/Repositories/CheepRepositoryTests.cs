using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests.Repositories;

public class CheepRepositoryTests
{
    private readonly CheepDbContext _cheepDbContext;
    private readonly CheepRepository _cheepRepository;

    public CheepRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseInMemoryDatabase(databaseName: "CheepRepositoryTests")
            .Options;

        _cheepDbContext = new CheepDbContext(options);
        DbInitializer.SeedDatabase(_cheepDbContext);

        _cheepRepository = new CheepRepository(_cheepDbContext);
    }

    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 32)]
    [InlineData(3, 0)]
    public async Task GetCheeps(int page, int expected)
    {
        var cheeps = await _cheepRepository.GetCheeps(page);

        Assert.Equal(expected, cheeps.Count);
    }

    [Theory]
    [InlineData("first", 1, 2)]
    [InlineData("watch", 1, 2)]
    [InlineData("at", 1, 32)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var cheeps = (await _cheepRepository.SearchCheeps(search, page)).ToList();

        Assert.Equal(expected, cheeps.Count);
    }
}
