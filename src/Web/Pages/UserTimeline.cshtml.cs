using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDB.Records;
using Web.Interfaces;

namespace Web.Pages;

public class UserTimelineModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;
    public List<Cheep> Cheeps { get; set; } = [];

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
