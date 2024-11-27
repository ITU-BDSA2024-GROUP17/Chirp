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
        DbInitializer.SeedDatabase(_cheepDbContext);

        _authorRepository = new AuthorRepository(_cheepDbContext);
    }

    [Fact]
    public async Task GetAllAuthorsTest()
    {
        var authors = await _authorRepository.GetAuthors(1);

        Assert.Equal(4, authors.Count);
    }


    [Theory]
    [InlineData("John Doe")]
    [InlineData("Jane Smith")]
    [InlineData("John")]
    public async Task GetAuthorByNameTest(string searchName)
    {
        var author = await _authorRepository.GetAuthorByName(searchName);

        if (searchName.Equals("John"))
        {
            Assert.Null(author);
        }
        else
        {
            Assert.Equal(searchName, author?.UserName);
        }

    }


    [Theory]
    // Setup: InlineData("Search", "Expected")
    [InlineData("", new[] { "John Doe", "John Smith", "Jane Smith", "Jane Doe" })]
    [InlineData("t", new[] { "Jane Smith", "John Smith" })] // 't' only in Smith
    [InlineData("John", new[] { "John Doe", "John Smith" })]
    [InlineData("smith", new[] { "Jane Smith", "John Smith" })]
    public async Task SearchAuthorsTest(string search, string[] expected)
    {
        var authors = await _authorRepository.SearchAuthors(search, 1);

        Assert.Equal(expected.Length, authors.Count);

        foreach (var item in authors)
        {
            Assert.Contains(item.UserName, expected);
        }
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Jane Smith")]
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

    [Theory]
    [InlineData("John Doe")]
    [InlineData("John Smith")]
    public async Task GetCheepsWithLikesByAuthorTest(string name)
    {
        var author = await _authorRepository.GetAuthorByName(name) ?? throw new Exception("Author not found");

        foreach (var cheep in author.Cheeps)
        {
            if (cheep.Id == 1)
            {
                Assert.Single(cheep.Likes);
            }
            else if (cheep.Id == 3)
            {
                Assert.Equal(2, cheep.Likes.Count);

            }
            else
            {
                Assert.Empty(cheep.Likes);
            }
        }
    }
}
