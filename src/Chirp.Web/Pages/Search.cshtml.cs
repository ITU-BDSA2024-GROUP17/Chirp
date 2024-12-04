using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Chirp.Infrastructure.Services;
using Chirp.Core.Interfaces;

namespace Chirp.Web.Pages;
[BindProperties]
public class SearchModel(AuthorService authorService, CheepService cheepService) : PageModel, ICheepModel
{

    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;

    public IEnumerable<Author> Authors { get; set; } = [];
    public IEnumerable<Cheep> Cheeps { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }
    public string CheepMessage { get; set; } = "";

    /// <summary>
    /// Retrieves the Authors and Cheeps for the current page.
    /// </summary>
    /// <param name="SearchQuery"></param>
    /// <param name="page"></param>
    /// <returns>The page containing the search result.</returns>
    public async Task<IActionResult> OnGet([FromQuery] string SearchQuery, [FromQuery] int page = 1)
    {
        if (string.IsNullOrEmpty(SearchQuery))
        {
            return Page();
        }
        // Redirect if user try 0 or negative page
        if (page < 1)
        {
            return Redirect($"/search?SearchQuery={SearchQuery}&page=1");
        }

        // Focus search by symbol
        switch (SearchQuery.First())
        {
            // Search by Author
            case '@':
                Authors = await _authorService.SearchAuthors(SearchQuery.Split("@")[1], page);
                break;
            // Search by Cheep
            case '#':
                Cheeps = await _cheepService.SearchCheeps(SearchQuery.Split("#")[1], page);
                foreach (var c in Cheeps)
                {
                    Authors.Append(c.Author);
                }
                break;
            // Search by both
            default:
                Authors = await _authorService.SearchAuthors(SearchQuery, page);
                Cheeps = await _cheepService.SearchCheeps(SearchQuery, page);
                break;
        }
        return Page();
    }
}
