namespace API.Entities;

public class Following
{
    public int Id { get; set; }
    public int FollowedUserId { get; set; }
    public virtual User FollowedUser { get; set; } = null!;
    public int FollowingUserId { get; set; }
    public virtual User FollowingUser { get; set; } = null!;
    public int Status { get; set; }
}