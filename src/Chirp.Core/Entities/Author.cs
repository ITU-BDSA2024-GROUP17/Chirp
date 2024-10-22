using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Author
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; } = [];
}
