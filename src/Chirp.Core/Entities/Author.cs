using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Author : IdentityUser
{
    public string? Avatar { get; set; }
    public ICollection<Cheep> Cheeps { get; set; } = [];
    public ICollection<Cheep> LikedCheeps { get; set; } = [];
    public ICollection<Author> Following { get; set; } = [];
    public ICollection<Author> Followers { get; set; } = [];
}
