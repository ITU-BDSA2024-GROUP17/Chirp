using Web.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages;

public class SearchModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;

    public ICollection<Author> Authors { get; set; } = [];
    public ICollection<Cheep> Cheeps { get; set; } = [];


    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }

    public ActionResult OnGet([FromQuery] string SearchQuery)
    {
        if (SearchQuery == null)
        {
            return Page();
        }
        else if (SearchQuery.First() == '@')
        {
            Authors = _service.GetCheeps(SearchQuery.Split("@")[1]).Item1;
            return Page();
        }
        else
        {
            Authors = _service.GetCheeps(SearchQuery).Item1;
            Cheeps = _service.GetCheeps(SearchQuery).Item2;
        }

        return Page();
    }
}
