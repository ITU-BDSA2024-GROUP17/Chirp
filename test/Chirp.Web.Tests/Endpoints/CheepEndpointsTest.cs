using Chirp.Core.DTOs;
using Chirp.Core.Entities;
using Chirp.Web.Tests.Integration;
using Newtonsoft.Json;
namespace Chirp.Web.Tests.Endpoints;
/// <summary>
/// Integration tests for web endpoints related to searching authors.
/// </summary>
public class WebEndpointsTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    public readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebEndpointsTest"/> class.
    /// Seeds the test database and sets up the web application factory.
    /// </summary>
    /// <param name="factory">The custom web application factory.</param>
    public WebEndpointsTest(CustomWebApplicationFactory<Program> factory)
    {
        CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);
        _factory = factory;
    }

    /// <summary>
    /// Tests the search field endpoint with a single query.
    /// </summary>
    /// <param name="SearchQuery">The search query string.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Theory]
    [InlineData("John")]
    [InlineData("Doe")]
    public async Task SearchFieldTest(string SearchQuery)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/searchField?searchQuery={SearchQuery}&page=1");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var authors = JsonConvert.DeserializeObject<List<CreateAuthorDto>>(json);
        Assert.NotNull(authors);
        Assert.NotEmpty(authors);
        Assert.IsType<List<CreateAuthorDto>>(authors);
        Assert.IsNotType<List<Author>>(authors);
        Assert.Contains(authors, a => a.UserName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests the search field endpoint with multiple queries.
    /// </summary>
    /// <param name="SearchQuery">The search query string.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Theory]
    [InlineData("Jane")]
    [InlineData("Doe")]
    public async Task SearchFieldTest_MultipleQueries(string SearchQuery)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/searchField?searchQuery={SearchQuery}&page=1");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var authors = JsonConvert.DeserializeObject<List<CreateAuthorDto>>(json);
        Assert.NotNull(authors);
        Assert.NotEmpty(authors);
        Assert.IsType<List<CreateAuthorDto>>(authors);
        Assert.IsNotType<List<Author>>(authors);
        Assert.Contains(authors, a => a.UserName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests the search field endpoint with an empty query.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    // [Fact]
    // public async Task SearchFieldTest_EmptyQuery()
    // {
    //     var client = _factory.CreateClient();
    //     var response = await client.GetAsync("/searchField?searchQuery=&page=1");
    //     response.EnsureSuccessStatusCode();
    //     var json = await response.Content.ReadAsStringAsync();
    //     var authors = JsonConvert.DeserializeObject<List<CreateAuthorDto>>(json);
    //     Assert.NotNull(authors);
    //     Assert.Equal(4, authors.Count()); // Total of 4 authors in test db
    // }
}
