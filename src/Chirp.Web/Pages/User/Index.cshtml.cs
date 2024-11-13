using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;
using Chirp.Core.Interfaces;
using Chirp.Web.Interfaces.Pages;

namespace Chirp.Web.Pages.User;

public class UserModel(AuthorService authorService, CheepService cheepService) : PageModel, IUserPage, ICheepModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;

    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
    public IEnumerable<Cheep> Cheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }

    [BindProperty]
    public string CheepMessage { get; set; } = "";

    public async Task<IActionResult> OnGet(string author, [FromQuery] int page)
    {
        if (page < 1)
        {
            return Redirect($"/{author}?page=1");
        }

        Author = await _authorService.GetAuthorByName(author);

        if (Author != null)
        {
            if (Author.UserName == author)
            {
                Cheeps = await _authorService.GetCheepsTimeline(author, page);
                TotalCheeps = await _authorService.GetCheepsTimelineCount(author);
            }
            else
            {
                Cheeps = await _authorService.GetCheeps(author, page);
                TotalCheeps = await _authorService.GetCheepsCount(author);
            }

            TotalLikedCheeps = await _authorService.GetLikedCount(author);

            Following = await _authorService.GetFollowing(Author.Id);
            Followers = await _authorService.GetFollowers(Author.Id);
        }

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

    public async Task<IActionResult> OnPostCheepAsync(string returnUrl)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _authorService.GetAuthor(UserId) ?? throw new Exception("User not found!");

        try
        {
            Cheep cheep = new()
            {
                AuthorId = UserId,
                Message = CheepMessage,
                TimeStamp = DateTime.Now,
                Author = author,
                Likes = []
            };

            await _cheepService.CreateCheep(cheep);

            // Reload page
            return LocalRedirect(Url.Content($"~/{author}"));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content($"~/{author}"));
        }
    }
}
