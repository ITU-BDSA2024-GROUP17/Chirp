using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Author : IdentityUser
{
    public ICollection<Cheep> Cheeps { get; set; } = [];
}
