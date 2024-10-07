using Web.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
            var result = await _service.GetCheeps(SearchQuery.Split("@")[1], chunkSize);
            Authors = result.Item1;
            return Page();
        }
        else
        {
            var res = await _service.GetCheeps(SearchQuery, chunkSize);
            Authors = res.Item1;
            Cheeps = res.Item2;
            return Page();
        }
    }
}
