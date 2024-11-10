using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using Chirp.Core.DTOs;
using Chirp.Core.Entities;
using Chirp.Web;
using Chirp.Web.Tests.Integration;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit.Sdk;
namespace Chirp.Web.Tests.Endpoints;

public class WebEndpointsTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    public readonly CustomWebApplicationFactory<Program> _factory;
    public WebEndpointsTest(CustomWebApplicationFactory<Program> factory)
    {
        CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);


        _factory = factory;
    }

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
