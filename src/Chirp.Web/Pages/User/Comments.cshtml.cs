using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;
using Chirp.Web.Interfaces.Pages;
using Chirp.Core.Interfaces;

namespace Chirp.Web.Pages.User;

public class UserCommentsModel(AuthorService authorService, CheepService cheepService) : PageModel, IUserPage, ICheepModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;

    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
    public IEnumerable<Cheep> CommentedCheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalCommentedCheeps { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; } = [];
    public string CheepMessage { get; set; } = "";

    public async Task<IActionResult> OnGet(string author, [FromQuery] int page)
    {
        if (page < 1)
        {
            return Redirect($"{Request.Path}?page=1");
        }

        Author = await _authorService.GetAuthorByName(author);
        CommentedCheeps = await _authorService.GetCheepsCommented(author, page);
        TotalCommentedCheeps = await _authorService.GetCheepsCommentedCount(author);
        Cheeps = CommentedCheeps;

        if (Author != null)
        {
            if (Author.UserName == author)
            {
                TotalCheeps = await _authorService.GetCheepsTimelineCount(author);
            }
            else
            {
                TotalCheeps = await _authorService.GetCheepsCount(author);
            }

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
}
