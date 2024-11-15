namespace Chirp.Core.Entities;

public class AuthorFollow
{
    public string FollowerId { get; set; } = null!;
    public Author Follower { get; set; } = null!;

    public string FolloweeId { get; set; } = null!;
    public Author Followee { get; set; } = null!;

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}
