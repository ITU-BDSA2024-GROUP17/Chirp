using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using Chirp.Web.Interfaces.Pages;

namespace Chirp.Web.Pages.User;

public class UserLikesModel(AuthorService authorService) : PageModel, IUserPage
{
    private readonly AuthorService _authorService = authorService;

    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
    public ICollection<Cheep> LikedCheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }
    public int TotalCommentedCheeps { get; set; }

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

        if (Author != null)
        {
            if (@User.Identity?.Name == author)
            {
                TotalCheeps = await _authorService.GetCheepsTimelineCount(author);
            }
            else
            {
                TotalCheeps = await _authorService.GetCheepsCount(author);
            }

            LikedCheeps = await _authorService.GetLiked(Author.Id, page);

            TotalLikedCheeps = await _authorService.GetLikedCount(Author.Id);
            TotalCommentedCheeps = await _authorService.GetCheepsCommentedCount(Author.Id);

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
