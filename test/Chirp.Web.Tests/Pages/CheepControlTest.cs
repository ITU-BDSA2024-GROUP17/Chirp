using AngleSharp.Common;
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

    [Fact]
    public async Task DeleteCheepTest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CheepDbContext>();

        var user = context.Authors.Search("John Doe", a => a.UserName!).FirstOrDefault(); // Replace with a valid user authentication token

        var cheepId = context.Cheeps.Search(user!.Id, c => c.AuthorId).FirstOrDefault(); // Replace with a valid cheep ID


        // Act
        var response = await client.PostAsync($"/?UserAuth={user.Id}&cheepId={cheepId}&handler=Delete", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("Cheep not found for delete!", responseString);
        Assert.DoesNotContain("User can't delete this cheep", responseString);
    }
}
