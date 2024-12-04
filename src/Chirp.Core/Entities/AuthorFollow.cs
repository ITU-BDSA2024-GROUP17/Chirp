namespace Chirp.Core.Entities;

public class AuthorFollow
{
    /// <summary>
    /// Id of the author that is following.
    /// </summary>
    public string FollowerId { get; set; } = null!;
    /// <summary>
    /// Navigation to the author that is following.
    /// </summary>
    public Author Follower { get; set; } = null!;

    /// <summary>
    /// Id of the author that is being followed.
    /// </summary>
    public string FolloweeId { get; set; } = null!;
    /// <summary>
    /// Navigation to the author that is being followed.
    /// </summary>
    public Author Followee { get; set; } = null!;

    /// <summary>
    /// Timestamp of when the follower started to follow the followee.
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}
