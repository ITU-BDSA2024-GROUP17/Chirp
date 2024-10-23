using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests.Repositories;

public class AuthorRepositoryTests
{
    private readonly CheepDbContext _cheepDbContext;
    private readonly AuthorRepository _authorRepository;

    public AuthorRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthorRepositoryTests")
            .Options;

        _cheepDbContext = new CheepDbContext(options);
        DbInitializer.SeedDatabase(_cheepDbContext);

        _authorRepository = new AuthorRepository(_cheepDbContext);
    }

    [Fact]
    public async Task GetAllAuthors()
    {
        var authors = await _authorRepository.GetAuthors(1);

        Assert.Equal(4, authors.Count);
    }

    [Theory]
    [InlineData("John", 1, 2)]
    [InlineData("o", 1, 3)]
    [InlineData("smith", 1, 2)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var authors = (await _authorRepository.SearchAuthors(search, page)).ToList();

        Assert.Equal(expected, authors.Count);
    }
}
