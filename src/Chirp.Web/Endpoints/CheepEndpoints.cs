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
            List<Author> authors = await cheepService.SearchAuthors(SearchQuery, 1);

            List<CreateAuthorDto> authorDtos = new List<CreateAuthorDto>();
            foreach (var author in authors)
            {
                authorDtos.Add(new CreateAuthorDto()
                {
                    Id = author.Id,
                    UserName = author.UserName ?? "",
                });
            }


            return authorDtos;
        }).WithSummary("Gets all authors");
    }
}
