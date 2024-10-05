using System.ComponentModel.DataAnnotations;

namespace Web.Entities;

public class Author
{
    [Key]
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required long TimeOfCreation { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; }
}
