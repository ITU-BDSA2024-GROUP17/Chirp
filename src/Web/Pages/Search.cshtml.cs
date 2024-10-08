using Web.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages;
[BindProperties]
public class SearchModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;

    public IEnumerable<Author> Authors { get; set; } = [];
    public IEnumerable<Cheep> Cheeps { get; set; } = [];


    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }


    public async Task<IActionResult> OnGet([FromQuery] string SearchQuery, [FromQuery] int page = 1)
    {
        SearchQuery = SearchQuery.Split("?")[0];
        // Redirect if user try 0 or negative page
        if (page < 1)
        {
            return Redirect($"/search?SearchQuery={SearchQuery}&page=1");
        }
        if (string.IsNullOrEmpty(SearchQuery))
        {
            return Page();
        }


        switch (SearchQuery.First())
        {
            case '@':
                Authors = await _service.SearchAuthors(SearchQuery.Split("@")[1], page);
                break;
            case '&':
                Cheeps = await _service.SearchCheeps(SearchQuery.Split("&")[1], page);
                break;
            default:
                Authors = await _service.SearchAuthors(SearchQuery, page);
                Cheeps = await _service.SearchCheeps(SearchQuery, page);
                break;
        }
        return Page();
    }
}
