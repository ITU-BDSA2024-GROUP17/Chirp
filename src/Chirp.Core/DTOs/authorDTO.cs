namespace Chirp.Core.DTOs;

public class CreateAuthorDto
{
    /// <summary>
    /// Id of the author.
    /// </summary>
    public required string Id { get; set; }
    /// <summary>
    /// Username of the author.
    /// </summary>
    public required string UserName { get; set; }
    /// <summary>
    /// URL to the author's avatar image.
    /// </summary>
    public string? Avatar { get; set; }
}
