using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class CheepRevision
{
    /// <summary>
    /// Id to the revision.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// The message the that this revision contains.
    /// The message is limited to at most 160 characters.
    /// </summary>
    [StringLength(160)]
    public required string Message { get; set; }
    /// <summary>
    /// Timestamp of when this revision was made.
    /// This can be used to determine when an edit to a cheep is made.
    /// </summary>
    public required DateTime TimeStamp { get; set; }
}
