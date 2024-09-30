using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDB.Records;
using CSVDBService.Interfaces;

namespace CSVDBService.Pages;

public class PublicModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;
    public List<Cheep> Cheeps { get; set; } = [];

    public ActionResult OnGet([FromQuery] int page)
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
