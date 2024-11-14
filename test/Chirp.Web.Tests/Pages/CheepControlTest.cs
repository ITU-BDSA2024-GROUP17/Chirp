using AngleSharp.Common;
using AngleSharp.Html.Parser;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Extensions;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace Chirp.Web.Tests.Integration;

/// <summary>
/// General page endpoint tests and client load tests.
/// </summary>
public class CheepControlTest
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    public CheepControlTest(CustomWebApplicationFactory<Program> factory)
    {
        CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);
        _factory = factory;
    }
    // [Fact]
    // public async Task DeleteCheepTest()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     using var scope = _factory.Services.CreateScope();
    //     var context = scope.ServiceProvider.GetRequiredService<CheepDbContext>();

    //     var user = context.Authors.FirstOrDefault(a => a.UserName == "John Doe"); // Replace with a valid user authentication token
    //     if (user == null) throw new Exception("User not found");

    //     var cheep = context.Cheeps.FirstOrDefault(c => c.AuthorId == user.Id); // Replace with a valid cheep ID
    //     if (cheep == null) throw new Exception("Cheep not found");

    //     // Get the anti-forgery token
    //     var getResponse = await client.GetAsync("/");
    //     getResponse.EnsureSuccessStatusCode();
    //     var getResponseString = await getResponse.Content.ReadAsStringAsync();

    //     var parser = new HtmlParser();
    //     var document = parser.ParseDocument(getResponseString);
    //     var tokenElement = document.QuerySelector("input[name=__RequestVerificationToken]");
    //     var token = tokenElement?.GetAttribute("value");

    //     if (token == null) throw new Exception("Anti-forgery token not found");

    //     var formData = new Dictionary<string, string>
    // {
    //     { "UserAuth", user.Id },
    //     { "cheepId", cheep.Id.ToString() },
    //     { "handler", "Delete" },
    //     { "__RequestVerificationToken", token }
    // };

    //     var content = new FormUrlEncodedContent(formData);

    //     // Act
    //     var response = await client.PostAsync("/", content);

    //     // Log the response for debugging
    //     var responseString = await response.Content.ReadAsStringAsync();
    //     Console.WriteLine(responseString);

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     Assert.DoesNotContain("Cheep not found for delete!", responseString);
    //     Assert.DoesNotContain("User can't delete this cheep", responseString);
    // }
}
