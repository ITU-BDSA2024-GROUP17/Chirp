using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using AngleSharp;

namespace Chirp.Web.Tests.Integration;

/// <summary>
/// General page endpoint tests and client load tests.
/// </summary>
public class PageSetupTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PageSetupTests(CustomWebApplicationFactory<Program> factory)
    {
        CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);
        _factory = factory;
    }

    private static AngleSharp.Dom.IDocument LoadDocument(HttpResponseMessage request)
    {
        var html = request.Content.ReadAsStringAsync().Result;
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        return context.OpenAsync(req => req.Content(html)).Result;
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

        var page = LoadDocument(response);
        var pageTitle = page.QuerySelector("title")?.TextContent;
        if (url == "/")
        {
            Assert.Contains("Public Timeline", pageTitle);
            return;
        }
        else
        {
            Assert.Contains(url.Split("/")[1], pageTitle);
        }
    }
    [Theory]
    [InlineData("/")]
    [InlineData("/RandomUser")]
    [InlineData("/Helge")]
    [InlineData("/NonExistingPage")]
    public async Task Get_IdentityEndpointsReturnSuccessAndCorrectContentType(string url)
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
