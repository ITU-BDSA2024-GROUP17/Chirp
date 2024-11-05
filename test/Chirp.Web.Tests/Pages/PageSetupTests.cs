using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Chirp.Web.Tests.Integration;
public class PageSetupTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PageSetupTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/RandomUser")]
    [InlineData("/Helge")]
    [InlineData("/NonExistingPage")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var pageContent = await response.Content.ReadAsStringAsync();
        Assert.Contains(url.Split("/")[1], pageContent);

    }
}
