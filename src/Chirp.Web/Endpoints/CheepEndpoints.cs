using Chirp.Core.DTOs;
using Chirp.Infrastructure.Services;

namespace Chirp.Web.Endpoints;

public static class CheepEndpoints
{
    public static void MapCheepEndpoints(this WebApplication app, CheepService cheepService)
    {
        app.MapPost("/cheep", (CreateCheepDto cheep) =>
        {
            cheepService.CreateCheep(cheep);

            return cheep;
        }).WithSummary("Sends a cheep");
    }
}
