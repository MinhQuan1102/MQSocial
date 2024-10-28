namespace API.Entities;

public class Reaction
{
    public int Id { get; set; }
    public int Type { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int? PostId { get; set; }
    public virtual Post Post { get; set; } = null!;
    public int? CommentId { get; set; }
    public virtual Comment Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}