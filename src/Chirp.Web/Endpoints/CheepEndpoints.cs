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
                    Name = cheepDto.Author,
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

            return cheep;
        }).WithSummary("Sends a cheep");
    }
}
