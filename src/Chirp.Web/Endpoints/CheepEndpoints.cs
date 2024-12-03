using Chirp.Core.DTOs;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;

namespace Chirp.Web.Endpoints;

public static class CheepEndpoints
{
    public static void MapCheepEndpoints(this WebApplication app)
    {
        app.MapGet("/searchField", async (string SearchQuery, AuthorService authorService) =>
        {
            List<Author> authors = await authorService.SearchAuthors(SearchQuery, 1);

            List<CreateAuthorDto> authorDtos = [];
            foreach (var author in authors)
            {
                authorDtos.Add(new CreateAuthorDto()
                {
                    Id = author.Id,
                    UserName = author.UserName ?? "",
                    Avatar = author.Avatar,
                });
            }

            return authorDtos;
        }).WithSummary("Gets all authors");
    }
}
