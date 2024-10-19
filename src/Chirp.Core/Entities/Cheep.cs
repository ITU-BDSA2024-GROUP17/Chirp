using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Cheep
{
    [Key]
    public required int Id { get; set; }
    public required int AuthorId { get; set; }
    public required string Message { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
}
