using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Author : IdentityUser
{
    [PersonalData]
    public string? Avatar { get; set; }
    [PersonalData]
    public ICollection<Cheep> Cheeps { get; set; } = [];
    [PersonalData]
    public ICollection<Cheep> LikedCheeps { get; set; } = [];
    [PersonalData]
    public ICollection<Author> Following { get; set; } = [];
    [PersonalData]
    public ICollection<Author> Followers { get; set; } = [];
}
