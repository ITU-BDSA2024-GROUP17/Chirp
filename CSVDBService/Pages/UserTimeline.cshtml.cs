using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDB.Records;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int page = 1)
    {
        if (page < 1)
        {
            return Redirect($"/{author}?page=1");
        }
        Cheeps = _service.GetCheepsFromAuthor(author, page).Result;
        return Page();
    }
}
