namespace Chirp.Core.Entities;

public class CheepLike
{
    /// <summary>
    /// Timestamp of when the author liked the cheep.
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}
