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

    [Fact]
    public void GetAllCheeps()
    {
        var cheeps = _cheepRepository.GetCheeps();

        Assert.Equal(10, cheeps.Count);
    }

    [Theory]
    [InlineData("first", 1, 2)]
    [InlineData("watch", 1, 1)]
    [InlineData("at", 1, 6)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var cheeps = (await _cheepRepository.SearchCheeps(search, page)).ToList();

        Assert.Equal(expected, cheeps.Count);
    }
}
