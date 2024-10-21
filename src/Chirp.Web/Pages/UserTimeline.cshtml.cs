using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;

namespace Chirp.Web.Pages;

public class UserTimelineModel(CheepService service) : PageModel
{
    private readonly CheepService _service = service;
    public IEnumerable<Cheep> Cheeps { get; set; } = [];

    public async Task<IActionResult> OnGet(string author, [FromQuery] int page = 1)
    {
        if (page < 1)
        {
            return Redirect($"/{author}?page=1");
        }
        Cheeps = await _service.GetCheepsFromAuthor(author, page);
        return Page();
    }
}
