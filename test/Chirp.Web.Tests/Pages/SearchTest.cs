
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
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace Chirp.Web.Tests.Integration;
public class SearchIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ICheepRepository> _mockCheepRepository;
    private readonly Mock<IAuthorRepository> _mockAuthorRepository;
    public static ILogger<ConsoleLoggerProvider> AppLogger = null!;
    public static ILoggerFactory loggerFactory = null!;

    public SearchIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _mockCheepRepository = new Mock<ICheepRepository>();
        _mockAuthorRepository = new Mock<IAuthorRepository>();
        _factory = factory.WithWebHostBuilder(builder =>
          {
              _ = builder.ConfigureServices(services =>
              {
                  // Replace the CheepDbContext with an in-memory database
                  services.RemoveAll(typeof(CheepDbContext));
                  services.AddDbContext<CheepDbContext>(options =>
                  {
                      options.UseLoggerFactory(loggerFactory);

                      options.UseInMemoryDatabase("InMemoryCheepDb");
                      options.UseInternalServiceProvider(services.BuildServiceProvider());
                  });

                  services.AddLogging(builder => builder
                       .AddConsole()
                       .AddFilter(level => level >= LogLevel.Trace)
                   );
                  loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>()!;
                  AppLogger = loggerFactory.CreateLogger<ConsoleLoggerProvider>();
                  // Replace the repositories with mock versions
                  services.RemoveAll<ICheepRepository>();
                  services.AddSingleton(_mockCheepRepository.Object);

                  services.RemoveAll<IAuthorRepository>();
                  services.AddSingleton(_mockAuthorRepository.Object);
              });
              builder.UseEnvironment("Development");

          });
    }

    [Fact]
    public async Task OnGet_SearchByAuthor_ReturnsAuthors()
    {
        // Arrange
        var mockAuthors = new List<Author> { new Author { UserName = "TestAuthor" } };
        _mockAuthorRepository.Setup(s => s.CreateAuthor(mockAuthors[0])).ReturnsAsync(mockAuthors[0]);
        _mockAuthorRepository.Setup(s => s.SearchAuthors("TestAuthor", 1)).ReturnsAsync(mockAuthors);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=TestAuthor&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _mockAuthorRepository.Verify(s => s.CreateAuthor(mockAuthors[0]), Times.Once);
        _mockAuthorRepository.Verify(s => s.SearchAuthors("TestAuthor", 1), Times.Once);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("TestAuthor", pageContent);
    }


    [Fact]
    public async Task OnGet_SearchByAuthor_NoResults()
    {
        // Arrange
        _mockAuthorRepository.Setup(s => s.SearchAuthors("NonExistentAuthor", 1)).ReturnsAsync(new List<Author>());

        // Act
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/search?SearchQuery=NonExistentAuthor&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("No Authors found", pageContent);
    }

    [Fact]
    public async Task OnGet_SearchByCheep_ReturnsCheeps()
    {
        // Arrange
        var mockCheep = new Cheep { AuthorId = "1", Author = new Author { UserName = "Author" }, Message = "TestCheep", TimeStamp = new DateTime(1970, 1, 1) };
        _mockCheepRepository.Setup(s => s.CreateCheep(mockCheep));

        var client = _factory.CreateClient();
        AppLogger.LogInformation("TestCheep");
        // Act
        var response = await client.GetAsync("/search?SearchQuery=&TestCheep&page=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Author", pageContent);
        Assert.Contains("TestCheep", pageContent);
    }


    [Fact]
    public async Task OnGet_SearchByCheep_NoResults()
    {
        // Arrange
        _mockCheepRepository.Setup(s => s.SearchCheeps("NonExistentCheep", 1)).ReturnsAsync(new List<Cheep>());

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/search?SearchQuery=&NonExistentCheep&page=1");
        var html = await response.Content.ReadAsStringAsync();
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
