using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Author : IdentityUser
{
    /// <summary>
    /// URL to the author's avatar image.
    /// </summary>
    [PersonalData]
    public string? Avatar { get; set; }
    /// <summary>
    /// A collection containing the cheeps sent by the author.
    /// </summary>
    [PersonalData]
    public ICollection<Cheep> Cheeps { get; set; } = [];
    /// <summary>
    /// A collection containing the cheeps liked by the author.
    /// </summary>
    [PersonalData]
    public ICollection<Cheep> LikedCheeps { get; set; } = [];
    /// <summary>
    /// A collection containing other authors, that is followed by this author.
    /// </summary>
    [PersonalData]
    public ICollection<Author> Following { get; set; } = [];
    /// <summary>
    /// A collection containing other authors, that is following this author.
    /// </summary>
    [PersonalData]
    public ICollection<Author> Followers { get; set; } = [];
}
