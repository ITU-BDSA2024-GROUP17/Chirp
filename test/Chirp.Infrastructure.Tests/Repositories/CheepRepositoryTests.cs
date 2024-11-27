using Chirp.Core.Entities;
using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests.Repositories;

public class CheepRepositoryTests
{
    private readonly CheepDbContext _cheepDbContext;
    private readonly CheepRepository _cheepRepository;
    private readonly AuthorRepository _authorRepository;

    public CheepRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseInMemoryDatabase(databaseName: "CheepRepositoryTests")
            .Options;

        _cheepDbContext = new CheepDbContext(options);
        DbInitializer.SeedDatabase(_cheepDbContext);

        _cheepRepository = new CheepRepository(_cheepDbContext);

        _authorRepository = new AuthorRepository(_cheepDbContext);
    }

    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 32)]
    public async Task GetCheeps(int page, int expected)
    {
        var cheeps = await _cheepRepository.GetCheeps(page);

        Assert.Equal(expected, cheeps.Count);
    }

    [Fact]
    public async Task DeleteCheep()
    {
        const int CHEEP_ID = 3;
        // Ensure cheep is avaliable for deletion
        var cheep = _cheepRepository.GetCheep(CHEEP_ID);
        Assert.True(cheep != null);

        await _cheepRepository.DeleteCheep(3);
        var cheepAfterDelete = await _cheepRepository.GetCheep(CHEEP_ID);
        Assert.True(cheepAfterDelete == null);

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

    [Theory]
    [InlineData(10, true)]
    [InlineData(159, true)]
    [InlineData(160, true)]
    [InlineData(161, false)]
    [InlineData(561, false)]
    public async Task SendLongCheep(int messageLength, bool shouldPass)
    {
        var author = await _authorRepository.GetAuthor("2bcf724c-b650-476c-ae11-d408eb2105a0");
        if (author == null) Assert.Fail("Author could not be found from db");
        var revision = new CheepRevision()
        {
            Message = "".PadRight(messageLength, 'A'),
            TimeStamp = DateTime.UtcNow
        };
        var revisionsList = new List<CheepRevision>();
        revisionsList.Add(revision);
        var cheep = new Cheep()
        {
            AuthorId = author.Id,
            Revisions = revisionsList,
            Author = author
        };
        try
        {
            await _cheepRepository.CreateCheep(cheep);
        }
        catch (InvalidDataException)
        {
            if (shouldPass) Assert.Fail("Cheep was not created, but it should have been");
            return;
        }
        if (!shouldPass) Assert.Fail("Cheep was created, but it shouldnt have");

    }
}
