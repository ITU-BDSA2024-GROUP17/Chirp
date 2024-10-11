using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Web.Interfaces;

namespace Web.Pages;

public class UserTimelineModel(ICheepService.IAuthor authorService) : PageModel
{
    private readonly ICheepService.IAuthor _authorService = authorService;
    public IEnumerable<Cheep> Cheeps { get; set; } = [];

    public async Task<IActionResult> OnGet(string author, [FromQuery] int page = 1)
    {
        if (page < 1)
        {
            return Redirect($"/{author}?page=1");
        }
        Cheeps = await _authorService.GetCheepsFromAuthor(author, page);
        return Page();
    }
}
