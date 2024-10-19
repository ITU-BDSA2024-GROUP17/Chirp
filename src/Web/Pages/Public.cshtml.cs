using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Entities;
using Web.Interfaces;
using Web.Services;

namespace Web.Pages;

public class PublicModel(CheepService service) : PageModel
{
    private readonly CheepService _service = service;
    public IEnumerable<Cheep> Cheeps { get; set; } = [];

    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        // Redirect if user try 0 or negative page
        if (page < 1)
        {
            return Redirect("/?page=1");
        }
        Cheeps = await _service.GetAllCheeps(page);
        return Page();
    }
}
