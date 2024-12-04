using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

public class Cheep
{
    /// <summary>
    /// Id of the cheep.
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Id of the author that created this cheep.
    /// </summary>
    public required string AuthorId { get; set; }
    /// <summary>
    /// A collection containing the revisions that this cheep have been through.
    /// A revision is added each time the cheep is edited such that a history can be kept.
    /// </summary>
    public required ICollection<CheepRevision> Revisions { get; set; } = [];
    /// <summary>
    /// Navigation to the author that created this cheep.
    /// </summary>
    public required Author Author { get; set; }
    /// <summary>
    /// A collection containing the authors that have liked the cheep.
    /// </summary>
    public ICollection<Author> Likes { get; set; } = [];
    /// <summary>
    /// A collection containing the authors that have commented on the cheep.
    /// </summary>
    public ICollection<Cheep> Comments { get; set; } = [];
    /// <summary>
    /// A navigation to the parent cheep.
    /// If this is blank that means that this is a ordinary cheep,
    /// where a non null value would mean that this cheep is a comment.
    /// </summary>
    public Cheep? CheepOwner { get; set; }
    /// <summary>
    /// The id of the parent cheep.
    /// If this is blank that means that this is a ordinary cheep,
    /// where a non null value would mean that this cheep is a comment.
    /// </summary>
    public int? CheepOwnerId { get; set; }
}
