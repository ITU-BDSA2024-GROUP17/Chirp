namespace Chirp.Core.DTOs;

public class CreateAuthorDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public string? Avatar { get; set; }
}
