using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;
using Chirp.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Chirp.Web.Pages;

public class PublicModel(AuthorService authorService, CheepService cheepService) : PageModel, ICheepModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;
    [BindProperty]
    public IEnumerable<Cheep> Cheeps { get; set; } = [];
    public int TotalCheeps { get; set; }

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
        TotalCheeps = await _cheepService.CountCheeps();
        return Page();
    }

    public async Task<IActionResult> OnPostCheepAsync(string returnUrl)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _authorService.GetAuthor(UserId) ?? throw new Exception("User not found!");

        var revisions = new List<CheepRevision>();


        try
        {
            CheepRevision revision = new()
            {
                Message = CheepMessage,
                TimeStamp = DateTime.Now
            };
            revisions.Add(revision);
            Cheep cheep = new()
            {
                AuthorId = UserId,
                Revisions = revisions,
                Author = author,
                Likes = []
            };

            await _cheepService.CreateCheep(cheep);

            // Reload page
            return LocalRedirect(Url.Content("~/"));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content("~/"));
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string UserAuth, int cheepId)
    {
        var cheep = await _cheepService.GetCheep(cheepId) ?? throw new Exception("Cheep not found for delete!");
        if (cheep.AuthorId.Equals(UserAuth))
        {
            await _cheepService.DeleteCheep(cheep.Id);
            return LocalRedirect("~/");
        }
        else
        {
            throw new Exception("User can't delete this cheep");
        }

    }

    public async Task<IActionResult> OnPostLikeAsync(int cheepId)
    {
        var cheep = await _cheepService.GetCheep(cheepId) ?? throw new Exception("Cheep not found for like!");

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

        return LocalRedirect(Url.Content("~/"));
    }

    public async Task<IActionResult> OnPostUpdateCheepAsync(int cheepId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");

        try
        {
            CheepRevision revision = new()
            {
                Message = CheepMessage,
                TimeStamp = DateTime.Now
            };

            await _cheepService.UpdateCheep(cheepId, revision);

            // Reload page
            return LocalRedirect(Url.Content("~/"));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content("~/"));
        }
    }

    public async Task<IActionResult> OnPostFollowAsync(string followeeId)
    {
        var FollowerId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");

        await _authorService.Follow(FollowerId, followeeId);

        return LocalRedirect(Url.Content("~/"));
    }

    public async Task<IActionResult> OnPostUnfollowAsync(string followeeId)
    {
        var FollowerId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");

        await _authorService.Unfollow(FollowerId, followeeId);

        return LocalRedirect(Url.Content("~/"));
    }

    public async Task<IActionResult> OnPostCommentCheepAsync(int CommentCheep, string CommentText)
    {
        if (string.IsNullOrWhiteSpace(CommentText))
        {
            ModelState.AddModelError("", "Comment cannot be empty.");
            return Page();
        }

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _authorService.GetAuthor(UserId) ?? throw new Exception("User not found!");



        try
        {

            Cheep cheep = new()
            {
                AuthorId = UserId,
                Author = author,
                Revisions = new List<CheepRevision>
                {
                    new CheepRevision
                    {
                        Message = CommentText,
                        TimeStamp = DateTime.UtcNow
                    }
                },
                Likes = []
            };

            await _cheepService.PostComment(CommentCheep, cheep);

            // Reload page
            return LocalRedirect(Url.Content("~/"));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content("~/"));
        }

    }


    public IActionResult OnPostPaginationAsync(int newPage)
    {
        return LocalRedirect(Url.Content($"~/?page={newPage}"));
    }
}
