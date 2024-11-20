using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class CheepRevision
{
    [Key]
    public Guid Id { get; set; }
    [StringLength(160)]
    public required string Message { get; set; }
    public required DateTime TimeStamp { get; set; }
}
