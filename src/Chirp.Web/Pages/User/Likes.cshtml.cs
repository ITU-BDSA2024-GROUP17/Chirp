using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using System.Security.Claims;
using Chirp.Web.Interfaces.Pages;
using Chirp.Core.Interfaces;

namespace Chirp.Web.Pages.User;

public class UserLikesModel(AuthorService authorService, CheepService cheepService) : PageModel, IUserPage, ICheepModel
{
    private readonly AuthorService _authorService = authorService;
    private readonly CheepService _cheepService = cheepService;

    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
    public IEnumerable<Cheep> LikedCheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; } = [];
    public string CheepMessage { get; set; } = "";

    /// <summary>
    /// Retrieves the Author and Cheeps for the current page.
    /// </summary>
    /// <param name="author">The requested author.</param>
    /// <param name="page">Page number to be retrieved.</param>
    /// <returns>The current page of the user's liked cheeps, or page 1 of the user's liked cheeps if current page is less than 1.</returns>
    public async Task<IActionResult> OnGet(string author, [FromQuery] int page)
    {
        if (page < 1)
        {
            return Redirect($"{Request.Path}?page=1");
        }

        Author = await _authorService.GetAuthorByName(author);
        LikedCheeps = await _authorService.GetLiked(author, page);
        TotalLikedCheeps = await _authorService.GetLikedCount(author);
        Cheeps = LikedCheeps;

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

    /// <summary>
    /// Paginate to a new page.
    /// </summary>
    /// <param name="newPage">The requested new page.</param>
    /// <returns>A redirect to the requested page.</returns>
    public IActionResult OnPostPaginationAsync(int newPage)
    {
        return Redirect($"{Request.Path}?page={newPage}");
    }
}
