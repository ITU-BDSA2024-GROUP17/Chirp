using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Cheep
{
    [Key]
    public int Id { get; set; }
    public required string AuthorId { get; set; }
    public required ICollection<CheepRevision> Revisions { get; set; } = [];
    public required Author Author { get; set; }
    public ICollection<Author> Likes { get; set; } = [];
}
