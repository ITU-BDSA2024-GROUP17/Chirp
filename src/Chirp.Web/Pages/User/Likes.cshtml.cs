using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;

namespace Chirp.Web.Pages.User;

public class UserLikesModel(AuthorService authorService, CheepService cheepService) : PageModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;

    public Author? Author { get; set; }
    public IEnumerable<Cheep> LikedCheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }

    public async Task<IActionResult> OnGet(string author, [FromQuery] int page)
    {
        if (page < 1)
        {
            return Redirect($"{Request.Path}?page=1");
        }

        Author = await _authorService.GetAuthorByName(author);
        TotalCheeps = await _authorService.GetCheepsCount(author);
        LikedCheeps = await _authorService.GetLiked(author, page);
        TotalLikedCheeps = await _authorService.GetLikedCount(author);
        return Page();
    }

    public async Task<IActionResult> OnPostLikeAsync(int cheepId, string returnUrl)
    {
        var cheep = await _cheepService.GetCheep(cheepId) ?? throw new Exception("Cheep not found!");

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _authorService.GetAuthor(UserId) ?? throw new Exception("User not found!");

        if (cheep.Likes.Contains(author))
        {
            await _cheepService.UnlikeCheep(cheepId, UserId);
        }
        else
        {
            await _cheepService.LikeCheep(cheepId, UserId);
        }

        return LocalRedirect(Request.Path.ToString());
    }

    public IActionResult OnPostPaginationAsync(int newPage)
    {
        return Redirect($"{Request.Path}?page={newPage}");
    }
}
