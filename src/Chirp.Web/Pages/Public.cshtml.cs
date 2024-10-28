using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;

namespace Chirp.Web.Pages;

public class PublicModel(CheepService service) : PageModel
{
    private readonly CheepService _cheepService = service;
    public IEnumerable<Cheep> Cheeps { get; set; } = [];

    [BindProperty]
    public string CheepMessage { get; set; } = "";

    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        // Redirect if user try 0 or negative page
        if (page < 1)
        {
            return Redirect("/?page=1");
        }
        Cheeps = await _cheepService.GetCheeps(page);
        return Page();
    }

    public async Task<IActionResult> OnPostCheepAsync(string returnUrl)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _cheepService.GetAuthor(UserId) ?? throw new Exception("User not found!");

        if (CheepMessage == null || CheepMessage.Length > 160) return LocalRedirect(Url.Content("~/"));

        Cheep cheep = new()
        {
            AuthorId = UserId,
            Message = CheepMessage,
            TimeStamp = DateTime.Now,
            Author = author,
        };

        await _cheepService.CreateCheep(cheep);

        // Reload page
        return LocalRedirect(Url.Content("~/"));
    }
}
