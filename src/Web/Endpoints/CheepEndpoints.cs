using web.DTOs;
using Web.Entities;
using Web.Interfaces;

namespace Web.Endpoints;

public static class CheepEndpoints
{

    public static void MapCheepEndpoints(this WebApplication app, ICheepService cheepService)
    {

        app.MapPost("/cheep", (CreateCheepDto cheep) =>
        {
            cheepService.StoreCheep(cheep);

            return cheep;
        }).WithSummary("Sends a cheep");
    }

}