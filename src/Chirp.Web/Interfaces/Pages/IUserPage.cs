using Chirp.Core.Entities;

namespace Chirp.Web.Interfaces.Pages;

interface IUserPage
{
    public Author? Author { get; set; }
    public int TotalCheeps { get; set; }
    public int TotalLikedCheeps { get; set; }
}
