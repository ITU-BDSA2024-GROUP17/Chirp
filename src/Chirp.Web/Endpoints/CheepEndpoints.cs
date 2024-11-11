using Chirp.Core.DTOs;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;

namespace Chirp.Web.Endpoints;

public static class CheepEndpoints
{
    public static void MapCheepEndpoints(this WebApplication app)
    {
        var _authorService = app.Services.CreateScope().ServiceProvider.GetService<AuthorService>() ?? throw new Exception("AuthorService not found!");

        app.MapGet("/searchField", async (string SearchQuery) =>
        {
            List<Author> authors = await _authorService.SearchAuthors(SearchQuery, 1);

            List<CreateAuthorDto> authorDtos = new List<CreateAuthorDto>();
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
