using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Cheep
{
    [Key]
    public int Id { get; set; }
    public required int AuthorId { get; set; }
    [StringLength(160)]
    public required string Message { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
}
