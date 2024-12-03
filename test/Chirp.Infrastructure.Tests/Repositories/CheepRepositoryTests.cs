using Chirp.Core.Entities;
using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests.Repositories;

[TestFixture]
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

    [TestCase(1, 32), Order(1)]
    [TestCase(2, 30)] // - 2 cheeps for 2 comments (comments gets saved as cheeps under a ´likes´ in another cheep)
    public async Task GetCheeps(int page, int expected)
    {
        var cheeps = await _cheepRepository.GetCheeps(page);
        Console.WriteLine(cheeps);
        Assert.That(cheeps, Has.Count.EqualTo(expected));
    }

    [Test, Order(999)]
    public async Task DeleteCheep()
    {
        const int CHEEP_ID = 3;
        // Ensure cheep is avaliable for deletion
        var cheep = _cheepRepository.GetCheep(CHEEP_ID);
        Assert.That(cheep, Is.Not.Null);

        await _cheepRepository.DeleteCheep(3);
        var cheepAfterDelete = await _cheepRepository.GetCheep(CHEEP_ID);
        Assert.That(cheepAfterDelete, Is.Null);
    }

    [TestCase("first", 1, 2)]
    [TestCase("watch", 1, 2)]
    [TestCase("at", 1, 32)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var cheeps = (await _cheepRepository.SearchCheeps(search, page)).ToList();

        Assert.That(cheeps, Has.Count.EqualTo(expected));
    }

    [TestCase(10, true)]
    [TestCase(159, true)]
    [TestCase(160, true)]
    [TestCase(161, false)]
    [TestCase(561, false)]
    public async Task SendLongCheep(int messageLength, bool shouldPass)
    {
        var author = await _authorRepository.GetAuthor("2bcf724c-b650-476c-ae11-d408eb2105a0");

        Assert.That(author, Is.Not.Null, "Author could not be found from db");

        var revision = new CheepRevision()
        {
            Message = "".PadRight(messageLength, 'A'),
            TimeStamp = DateTime.UtcNow
        };
        var revisionsList = new List<CheepRevision>
        {
            revision
        };
        var cheep = new Cheep()
        {
            AuthorId = author.Id, // Author cannot be null since it is asserted that earlier.
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

    [OneTimeTearDown]
    public void TearDown()
    {
        _cheepDbContext.Dispose();
    }
}
