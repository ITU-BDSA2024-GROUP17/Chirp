using Chirp.Core.Entities;
using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for CheepRepository.cs
/// </summary>
/// <remarks>
/// This class contains unit tests for the CheepRepository class.
/// TTests for updating/deleting cheeps, liking/unliking cheeps, commenting, search, and max length of cheeps.
/// </remarks>
/// <seealso cref="CheepRepository"/>
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

    /// <summary>
    /// Test GetCheeps method
    /// </summary>
    /// <remarks>
    ///  Note: Test case 2 is only 30 cheeps because two cheeps are comments on another cheep.
    /// </remarks>
    /// <param name="page">The pagination index</param>
    /// <param name="expected">Expected amount of cheeps on each page</param>
    /// <returns></returns>
    [TestCase(1, 32), Order(1)]
    [TestCase(2, 30)] // - 2 cheeps for 2 comments (comments gets saved as cheeps under a ´likes´ in another cheep)
    public async Task GetCheeps(int page, int expected)
    {
        var cheeps = await _cheepRepository.GetCheeps(page);
        Console.WriteLine(cheeps);
        Assert.That(cheeps, Has.Count.EqualTo(expected));
    }

    /// <summary>
    /// Test Update method
    /// </summary>
    [Test]
    public async Task UpdateCheep()
    {
        const int CHEEP_ID = 1;
        var cheep = await _cheepRepository.GetCheep(CHEEP_ID);
        var revision = new CheepRevision()
        {
            Message = "Updated message",
            TimeStamp = DateTime.UtcNow
        };
        var updatedCheep = await _cheepRepository.UpdateCheep(CHEEP_ID, revision);


        Assert.That(updatedCheep.Revisions, Has.Count.EqualTo(2));

        Assert.That(updatedCheep.Revisions.First().Message, Is.EqualTo(cheep!.Revisions.First().Message));
        Assert.That(updatedCheep.Revisions.Last().Message, Is.EqualTo("Updated message"));
    }

    /// <summary>
    /// Test Delete method
    /// </summary>
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

    /// <summary>
    /// Test Like and Unlike methods for cheeps
    /// </summary>
    [Test]
    public async Task LikeAndUnlikeCheep()
    {
        const int CHEEP_ID = 1;
        var cheep = await _cheepRepository.GetCheep(CHEEP_ID) ?? throw new Exception("Test: Cheep not found");
        var author = await _authorRepository.GetAuthor(cheep.AuthorId) ?? throw new Exception("Test: Author not found");

        // author already has a like on the cheep (expected = 1)
        Assert.That(cheep.Likes, Has.Count.EqualTo(1));

        await _cheepRepository.LikeCheep(CHEEP_ID, author.Id);
        // author liked his own cheep (expected = 2)
        Assert.That(cheep.Likes, Has.Count.EqualTo(2));

        /*
        *   Test Unlike of cheep
        */
        await _cheepRepository.UnlikeCheep(CHEEP_ID, author.Id);
        // author unliked his own cheep (expected = 1)
        Assert.That(cheep.Likes, Has.Count.EqualTo(1));
    }

    /// <summary>
    /// Test PostComment method
    /// </summary>
    /// <param name="i">The iteration for testcases</param>
    /// <param name="commentMessage">The string message to comment on the cheep</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [TestCase(1, "Comment 1")]
    [TestCase(2, "Comment 2")]
    public async Task PostComment(int i, string commentMessage)
    {
        const int CHEEP_ID = 1;
        var cheep = await _cheepRepository.GetCheep(CHEEP_ID) ?? throw new Exception("Test: Cheep not found");
        var author = await _authorRepository.GetAuthor(cheep.AuthorId) ?? throw new Exception("Test: Author not found");

        var comment = new Cheep()
        {
            Author = author,
            AuthorId = author.Id,
            Revisions = new List<CheepRevision>
            {
                new CheepRevision
                {
                    Message = commentMessage,
                    TimeStamp = DateTime.UtcNow
                }
            }
        };

        await _cheepRepository.PostComment(CHEEP_ID, comment);

        switch (i)
        {
            case 1:
                // c = 1 => 1 comment on cheep
                Assert.That(cheep.Comments, Has.Count.EqualTo(1));
                Assert.That(cheep.Comments.First().Revisions.First().Message, Is.EqualTo(commentMessage));
                break;
            case 2:
                // c = 2 => 2 comments on cheep
                Assert.That(cheep.Comments, Has.Count.EqualTo(2));
                Assert.That(cheep.Comments.Last().Revisions.First().Message, Is.EqualTo(commentMessage));
                break;
            default:
                throw new Exception("Invalid test case");
        }
    }

    /// <summary>
    /// Test SearchCheeps method
    /// </summary>
    /// <param name="search">Search query</param>
    /// <param name="page">pagination page</param>
    /// <param name="expected">Expected count of search results</param>
    /// <returns></returns>
    [TestCase("first", 1, 2)]
    [TestCase("watch", 1, 2)]
    [TestCase("at", 1, 32)]
    public async Task SearchCheeps(string search, int page, int expected)
    {
        var cheeps = (await _cheepRepository.SearchCheeps(search, page)).ToList();

        Assert.That(cheeps, Has.Count.EqualTo(expected));
    }

    /// <summary>
    /// Test CreateCheep method
    /// </summary>
    /// <param name="messageLength">The length of a cheep message</param>
    /// <param name="shouldPass">If the messageLength should be allowed</param>
    /// <returns></returns>
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
