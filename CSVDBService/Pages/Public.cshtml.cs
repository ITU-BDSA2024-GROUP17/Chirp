using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDB.Records;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    public ActionResult OnGet([FromQuery] int page = 1)
    {
        // Redirect if user try 0 or negative page
        if (page < 1)
        {
            return Redirect("/?page=1");
        }
        Cheeps = _service.GetCheeps(page).Result;
        return Page();
    }
}
