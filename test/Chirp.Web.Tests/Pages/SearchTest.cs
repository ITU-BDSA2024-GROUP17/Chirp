
using Chirp.Core.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Chirp.Core.Interfaces;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

public class SearchIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ICheepRepository> _mockCheepRepository;
    private readonly Mock<IAuthorRepository> _mockAuthorRepository;

    public SearchIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _mockCheepRepository = new Mock<ICheepRepository>();
        _mockAuthorRepository = new Mock<IAuthorRepository>();
        _factory = factory.WithWebHostBuilder(builder =>
          {
              builder.ConfigureServices(services =>
              {
                  // Replace the CheepDbContext with an in-memory database
                  services.RemoveAll(typeof(CheepDbContext));
                  services.AddDbContext<CheepDbContext>(options =>
                  {
                      options.UseInMemoryDatabase("InMemoryCheepDb");
                      options.UseInternalServiceProvider(services.BuildServiceProvider());
                  });

                  // Replace the repositories with mock versions
                  services.RemoveAll<ICheepRepository>();
                  services.AddSingleton(_mockCheepRepository.Object);

                  services.RemoveAll<IAuthorRepository>();
                  services.AddSingleton(_mockAuthorRepository.Object);
              });
              builder.UseEnvironment("Development"); // Enable detailed error messages

          });
    }

    [Fact]
    public async Task OnGet_SearchByAuthor_ReturnsAuthors()
    {
        // Arrange
        var mockAuthors = new List<Author> { new Author { UserName = "TestAuthor" } };
        _mockAuthorRepository.Setup(s => s.SearchAuthors("TestAuthor", 1)).ReturnsAsync(mockAuthors);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=@TestAuthor&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockAuthorRepository.Verify(s => s.SearchAuthors("TestAuthor", 1), Times.Once);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("TestAuthor", pageContent);
    }

    [Fact]
    public async Task OnGet_SearchByCheep_ReturnsCheeps()
    {
        // Arrange
        var mockCheeps = new List<Cheep> { new Cheep { AuthorId = "1", Author = new Author(), Message = "TestCheep", TimeStamp = new DateTime(1970, 1, 1) } };
        _mockCheepRepository.Setup(s => s.SearchCheeps("TestCheep", 1)).ReturnsAsync(mockCheeps);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=&TestCheep&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockCheepRepository.Verify(s => s.SearchCheeps("TestCheep", 1), Times.Once);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("TestCheep", pageContent);
    }

    [Fact]
    public async Task OnGet_SearchByAuthor_NoResults()
    {
        // Arrange
        _mockAuthorRepository.Setup(s => s.SearchAuthors("NonExistentAuthor", 1)).ReturnsAsync(new List<Author>());

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=&NonExistentAuthor&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockAuthorRepository.Verify(s => s.SearchAuthors("NonExistentAuthor", 1), Times.Never);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("NonExistentAuthor", pageContent);
        Assert.Contains("No Authors found", pageContent);
    }

    [Fact]
    public async Task OnGet_SearchByCheep_NoResults()
    {
        // Arrange
        _mockCheepRepository.Setup(s => s.SearchCheeps("NonExistentCheep", 1)).ReturnsAsync(new List<Cheep>());

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=NonExistentCheep&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockCheepRepository.Verify(s => s.SearchCheeps("NonExistentCheep", 1), Times.Never);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("NonExistentCheep", pageContent);
    }


    [Fact]
    public async Task OnGet_SearchByAuthorAndCheep_ReturnsResults()
    {
        // Arrange
        var mockAuthor = new Author { UserName = "TestAuthor" };
        var mockCheep = new Cheep { AuthorId = "1", Author = mockAuthor, Message = "TestCheep", TimeStamp = new DateTime(1970, 1, 1) };
        _mockAuthorRepository.Setup(s => s.CreateAuthor(mockAuthor)).ReturnsAsync(mockAuthor);
        _mockCheepRepository.Setup(s => s.CreateCheep(mockCheep)).ReturnsAsync(mockCheep);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=TestAuthor&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockAuthorRepository.Verify(s => s.SearchAuthors("TestAuthor", 1), Times.Once);
        _mockCheepRepository.Verify(s => s.SearchCheeps("TestCheep", 1), Times.Once);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("TestAuthor", pageContent);
        Assert.Contains("TestCheep", pageContent);
    }
}
