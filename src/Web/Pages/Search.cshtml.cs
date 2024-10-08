using Web.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages;

public class SearchModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;

    public IEnumerable<Author> Authors { get; set; } = [];
    public IEnumerable<Cheep> Cheeps { get; set; } = [];


    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }

    public int chunkSize { get; set; } = 25;

    public async Task<IActionResult> OnGet([FromQuery] string SearchQuery)
    {
        if (SearchQuery == null)
        {
            return Page();
        }
        else if (SearchQuery.First() == '@')
        {
            Authors = await _service.SearchAuthors(SearchQuery.Split("@")[1], chunkSize);
            return Page();
        }
        else
        {
            Cheeps = await _service.SearchCheeps(SearchQuery, chunkSize);
            return Page();
        }
    }
}
