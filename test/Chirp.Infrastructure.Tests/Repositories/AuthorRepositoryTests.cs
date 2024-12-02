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

    [Test]
    public async Task GetAllAuthors()
    {
        var authors = await _authorRepository.GetAuthors(1);

        Assert.That(authors, Has.Count.EqualTo(4));
    }

    [TestCase("John", 1, 2)]
    [TestCase("o", 1, 3)]
    [TestCase("smith", 1, 2)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var authors = (await _authorRepository.SearchAuthors(search, page)).ToList();

        Assert.That(authors, Has.Count.EqualTo(expected));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _cheepDbContext.Dispose();
    }
}
