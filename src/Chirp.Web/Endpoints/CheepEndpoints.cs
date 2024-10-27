using Chirp.Core.DTOs;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;

namespace Chirp.Web.Endpoints;

public static class CheepEndpoints
{
    public static void MapCheepEndpoints(this WebApplication app, CheepService cheepService)
    {
        app.MapPost("/cheep", (CreateCheepDto cheepDto) =>
        {

            var author = cheepService.GetAuthor(cheepDto.Author).Result;
            if (author == null)
            {
                author = new Author
                {
                    UserName = cheepDto.Author,
                    Email = ""
                };
                cheepService.CreateAuthor(author);
            }
            var cheep = new Cheep()
            {
                AuthorId = author.Id,
                Message = cheepDto.Message,
                TimeStamp = DateTime.Now,
                Author = author
            };
            cheepService.CreateCheep(cheep);

            return cheepDto;
        }).WithSummary("Sends a cheep");

        app.MapGet("/searchField", async (string SearchQuery) =>
        {
            return await cheepService.SearchAuthors(SearchQuery, 1);
        }).WithSummary("Gets all authors");
    }
}
