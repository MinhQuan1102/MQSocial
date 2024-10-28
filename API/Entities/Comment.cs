namespace API.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsEdited { get; set; }
    public int TotalReactions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public int? ParentCommentId { get; set; }
    public virtual Comment ParentComment { get; set; } = null!;
    public ICollection<Comment> ChildComments { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int PostId { get; set; }
    public virtual Post Post { get; set; } = null!;
}