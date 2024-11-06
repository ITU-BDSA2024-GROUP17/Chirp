// test/Chirp.Web.Tests/Pages/SearchIntegrationTests.cs
using Chirp.Core.Entities;

using Chirp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Chirp.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Chirp.Web.Tests.Integration
{
    public class SearchIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public SearchIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);
            _factory = factory;
        }


        [Fact]
        public async Task OnGet_SearchByAuthor_ReturnsAuthors()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/search?SearchQuery=J&page=1");
            var page = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("John Smith", page);
        }

        // [Fact]
        // public async Task OnGet_SearchByCheep_ReturnsCheeps()
        // {
        //     // Arrange
        //     var mockAuthor = new Author { UserName = "TestAuthor" };
        //     var mockCheep = new Cheep { AuthorId = "1", Author = mockAuthor, Message = "TestCheep", TimeStamp = new DateTime(1970, 1, 1) };
        //     _factory.MockCheepRepository.Setup(s => s.SearchCheeps("TestCheep", 1)).ReturnsAsync(new List<Cheep> { mockCheep });

        //     var client = _factory.CreateClient();

        //     // Act
        //     await client.GetAsync("/search?SearchQuery=TestCheep&page=1");

        //     // Assert
        //     _factory.MockCheepRepository.Verify(v => v.SearchCheeps("TestCheep", 1), Times.Once);
        //     _factory.MockCheepRepository.Verify(v => v.SearchCheeps("TestCheep", 2), Times.Never);
        // }

        // [Fact]
        // public async Task OnGet_SearchByAuthorAndCheep_ReturnsResults()
        // {
        //     // Arrange
        //     var mockAuthor = new Author { UserName = "TestAuthor" };
        //     var mockCheep = new Cheep { AuthorId = "1", Author = mockAuthor, Message = "TestCheep", TimeStamp = new DateTime(1970, 1, 1) };
        //     _factory.MockAuthorRepository.Setup(s => s.SearchAuthors("TestAuthor", 1)).ReturnsAsync(new List<Author> { mockAuthor });
        //     _factory.MockCheepRepository.Setup(s => s.SearchCheeps("TestCheep", 1)).ReturnsAsync(new List<Cheep> { mockCheep });
        //     var client = _factory.CreateClient();

        //     // Act
        //     await client.GetAsync("/search?SearchQuery=TestAuthor&page=1");

        //     // Assert
        //     _factory.MockAuthorRepository.Verify(v => v.SearchAuthors("TestAuthor", 1), Times.Once);
        //     // _factory.MockCheepRepository.Verify(v => v.SearchCheeps("TestCheep", 1), Times.Once);
        // }
    }
}
