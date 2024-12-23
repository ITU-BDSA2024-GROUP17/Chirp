using Chirp.Core.Entities;
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
        TestInfrastructureDbInitializer.SeedDatabase(_cheepDbContext);

        _authorRepository = new AuthorRepository(_cheepDbContext);
    }

    [Test]
    public async Task GetAllAuthors()
    {
        var authors = await _authorRepository.GetAuthors(1);

        Assert.That(authors, Has.Count.EqualTo(4));
    }

    /// <summary>
    /// Test if the author is found by the name
    /// </summary>
    /// <param name="searchName">The name to get from</param>
    /// <returns></returns>
    [TestCase("John Doe")]
    [TestCase("Jane Smith")]
    [TestCase("John")]
    public async Task GetAuthorByNameTest(string searchName)
    {
        var author = await _authorRepository.GetAuthorByName(searchName);

        if (searchName.Equals("John"))
        {
            Assert.Null(author);
        }
        else
        {
            Assert.That(author?.UserName, Is.EqualTo(searchName));
        }

    }

    /// <summary>
    /// Test if the author is found by a search query
    /// </summary>
    /// <param name="search">Search query</param>
    /// <param name="expected">Expected author results</param>
    // Setup: InlineData("Search", "Expected")
    [TestCase("", new[] { "John Doe", "John Smith", "Jane Smith", "Jane Doe" })]
    [TestCase("t", new[] { "Jane Smith", "John Smith" })] // 't' only in Smith
    [TestCase("John", new[] { "John Doe", "John Smith" })]
    [TestCase("smith", new[] { "Jane Smith", "John Smith" })]
    public async Task SearchAuthorsTest(string search, string[] expected)
    {
        var authors = await _authorRepository.SearchAuthors(search, 1);

        Assert.That(authors.Count, Is.EqualTo(expected.Length));

        foreach (var item in authors)
        {
            Assert.Contains(item.UserName, expected);
        }
    }

    /// <summary>
    /// Test if cheeps are valid upon retrieval
    /// </summary>
    /// <param name="author"></param>
    [TestCase("John Doe")]
    [TestCase("Jane Smith")]
    public async Task GetCheepsTest(string author)
    {
        var cheeps = await _authorRepository.GetCheeps(author, 1);

        foreach (var cheep in cheeps)
        {
            Assert.True(cheep.Author.UserName == author);
            // Ensure all fileds in the cheeps are consturcted
            Assert.NotNull(cheep.Revisions);
            Assert.NotNull(cheep.AuthorId);
            Assert.NotNull(cheep.Likes);
            Assert.NotNull(cheep.Comments);
            Assert.NotNull(cheep.Revisions);
        }
    }

    /// <summary>
    /// Test if cheeps come with likes
    /// </summary>
    /// <param name="name">The author name to test cheeps from</param>
    /// <param name="cheepID">The cheepId to test for expected likes</param>
    /// <param name="expectedLikes">Amount of expected like on a cheepId</param>
    // (Author, cheepId, expectedLikes)
    [TestCase("John Doe", 1, 1)]
    [TestCase("John Smith", 3, 2)]
    [TestCase("Jane Smith", 4, 3)]
    public async Task GetCheepsWithLikesByAuthorTest(string name, int cheepID, int expectedLikes)
    {
        var author = await _authorRepository.GetAuthorByName(name) ?? throw new Exception("Author not found");

        foreach (var cheep in author.Cheeps)
        {
            if (cheep.Id == cheepID)
            {
                Assert.That(cheep.Likes.Count, Is.EqualTo(expectedLikes));
            }
            else
            {
                Assert.IsEmpty(cheep.Likes);
            }
        }
    }

    /// <summary>
    /// Test if the author has followers or is following
    /// </summary>
    /// <param name="name">The author to test</param>
    [TestCase("John Doe")]
    [TestCase("John Smith")]
    public async Task GetFollowersByAuthorTest(string name)
    {
        var author = await _authorRepository.GetAuthorByName(name) ?? throw new Exception("Author not found");

        if (name == "John Doe")
        {
            var followers = await _authorRepository.GetFollowers(author.Id);
            Assert.That(followers.Count, Is.EqualTo(1));
            Assert.IsEmpty(await _authorRepository.GetFollowing(author.Id));
        }
        else if (name == "John Smith")
        {
            var followers = await _authorRepository.GetFollowing(author.Id);
            Assert.That(followers.Count, Is.EqualTo(1));
            Assert.IsEmpty(await _authorRepository.GetFollowers(author.Id));
        }
    }

    /// <summary>
    /// Test if a user can follow another
    /// </summary>
    /// <param name="follower">The author to test</param>
    /// <param name="followee">The author to test</param>
    [TestCase("John Smith", "John Doe")]
    [TestCase("Jane Smith", "Jane Doe")]
    public async Task FollowTest(string follower, string followee)
    {
        var author1 = await _authorRepository.GetAuthorByName(follower) ?? throw new Exception("Author not found");
        var author2 = await _authorRepository.GetAuthorByName(followee) ?? throw new Exception("Author not found");

        var followers = await _authorRepository.GetFollowers(author1.Id);
        Assert.That(followers, Has.Count.EqualTo(0));
        await _authorRepository.Follow(author2.Id, author1.Id);
        followers = await _authorRepository.GetFollowers(author1.Id);
        Assert.That(followers, Has.Count.EqualTo(1));
        Assert.That(followers[0], Is.EqualTo(author2));
        await _authorRepository.Unfollow(author2.Id, author1.Id);
        followers = await _authorRepository.GetFollowers(author1.Id);
        Assert.That(followers, Has.Count.EqualTo(0));
    }

    /// <summary>
    /// Unit test for searching authors
    /// </summary>
    /// <param name="search"></param>
    /// <param name="page"></param>
    /// <param name="expected"></param>
    /// <returns></returns>
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
