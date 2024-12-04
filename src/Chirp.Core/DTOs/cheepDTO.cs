namespace Chirp.Core.DTOs;

public class CreateCheepDto
{
    /// <summary>
    /// Author of the cheep.
    /// </summary>
    public required string Author { get; set; }
    /// <summary>
    /// Initial message in the cheep.
    /// </summary>
    public required string Message { get; set; }
}
