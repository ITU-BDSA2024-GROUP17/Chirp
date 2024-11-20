using Chirp.Core.Entities;

namespace Chirp.Web.Interfaces.Pages;

interface IUserPage
{
    public Author? Author { get; set; }
    public ICollection<Author> Following { get; set; }
    public ICollection<Author> Followers { get; set; }
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }
}
