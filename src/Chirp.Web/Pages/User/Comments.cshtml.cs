using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using Chirp.Web.Interfaces.Pages;

namespace Chirp.Web.Pages.User;

public class UserCommentsModel(AuthorService authorService) : PageModel, IUserPage
{
    private readonly AuthorService _authorService = authorService;

    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
    public ICollection<Cheep> CommentedCheeps { get; set; } = [];
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }
    public int TotalCommentedCheeps { get; set; }

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

            CommentedCheeps = await _authorService.GetCheepsCommented(Author.Id, page);

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
