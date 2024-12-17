using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;
using Chirp.Web.Interfaces;

namespace Chirp.Web.Pages;

public class PublicModel(AuthorService authorService, CheepService cheepService) : PageModel, ICheepModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;
    [BindProperty]
    public IEnumerable<Cheep> Cheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public string CheepMessage { get; set; } = "";

    /// <summary>
    /// Retrieves the Cheeps for the current page.
    /// </summary>
    /// <param name="page">Page number to be retrieved.</param>
    /// <returns>The requested page and all cheeps belonging to it.</returns>
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

    /// <summary>
    /// Post a new Cheep.
    /// </summary>
    /// <param name="cheepMessage">The message string of the cheep being created.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<IActionResult> OnPostCheepAsync([FromForm] string cheepMessage, string returnUrl = "/")
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");

        var UserId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");
        var author = await _authorService.GetAuthor(UserId) ?? throw new Exception("User not found!");

        var revisions = new List<CheepRevision>();

        try
        {
            CheepRevision revision = new()
            {
                Message = cheepMessage,
                TimeStamp = DateTime.UtcNow
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
            return LocalRedirect(Url.Content(returnUrl));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content(returnUrl));
        }
    }

    /// <summary>
    /// Delete a Cheep.
    /// </summary>
    /// <param name="UserAuth">The id of the user trying to delete the cheep.</param>
    /// <param name="cheepId">The id of the cheep to be deleted.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<IActionResult> OnPostDeleteAsync(string UserAuth, int cheepId, string returnUrl)
    {
        var cheep = await _cheepService.GetCheep(cheepId) ?? throw new Exception("Cheep not found for delete!");
        if (cheep.AuthorId.Equals(UserAuth))
        {
            await _cheepService.DeleteCheep(cheep.Id);
            return LocalRedirect(Url.Content(returnUrl));
        }
        else
        {
            throw new Exception("User can't delete this cheep");
        }

    }

    /// <summary>
    /// Like or unlike a Cheep.
    /// </summary>
    /// <param name="cheepId">The id of the cheep to be liked.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<IActionResult> OnPostLikeAsync(int cheepId, string returnUrl = "/")
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

        return LocalRedirect(Url.Content(returnUrl));
    }

    /// <summary>
    /// Update a Cheep.
    /// </summary>
    /// <param name="cheepId">The id of the cheep to be updated.</param>
    /// <param name="updateCheep">The updated message of the cheep.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<IActionResult> OnPostUpdateCheepAsync(int cheepId, [FromForm] string updateCheep, string returnUrl = "/")
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) throw new UnauthorizedAccessException("User is not logged in!");
        try
        {
            CheepRevision revision = new()
            {
                Message = updateCheep,
                TimeStamp = DateTime.Now
            };

            await _cheepService.UpdateCheep(cheepId, revision);

            // Reload page
            return LocalRedirect(Url.Content(returnUrl));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content(returnUrl));
        }
    }

    /// <summary>
    /// Follow a user.
    /// </summary>
    /// <param name="followeeId">The id of the user to be followed.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<IActionResult> OnPostFollowAsync(string followeeId, string returnUrl = "/")
    {
        var FollowerId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");

        await _authorService.Follow(FollowerId, followeeId);

        return LocalRedirect(Url.Content(returnUrl));
    }

    /// <summary>
    /// Unfollow a user.
    /// </summary>
    /// <param name="followeeId">The id of the followed user.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="Exception"></exception>/
    public async Task<IActionResult> OnPostUnfollowAsync(string followeeId, string returnUrl = "/")
    {
        var FollowerId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("User not found!");

        await _authorService.Unfollow(FollowerId, followeeId);

        return LocalRedirect(Url.Content(returnUrl));
    }

    /// <summary>
    /// Comment on a Cheep.
    /// </summary>
    /// <param name="CommentCheep">The id of the parent of the comment.</param>
    /// <param name="CommentText">The message of the comment.</param>
    /// <param name="returnUrl">The page to return to when the method finishes.</param>
    /// <returns>Page reload.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<IActionResult> OnPostCommentCheepAsync(int CommentCheep, string CommentText, string returnUrl = "/")
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
                Revisions =
                [
                    new CheepRevision
                    {
                        Message = CommentText,
                        TimeStamp = DateTime.UtcNow
                    }
                ],
                Likes = []
            };

            await _cheepService.PostComment(CommentCheep, cheep);

            // Reload page
            return LocalRedirect(Url.Content(returnUrl));
        }
        catch (InvalidDataException)
        {
            return LocalRedirect(Url.Content(returnUrl));
        }

    }

    /// <summary>
    /// Paginate to a new page.
    /// </summary>
    /// <param name="newPage">The requested new page.</param>
    /// <returns>A redirect to the requested page.</returns>
    public IActionResult OnPostPaginationAsync(int newPage)
    {
        return LocalRedirect(Url.Content($"~/?page={newPage}"));
    }
}
