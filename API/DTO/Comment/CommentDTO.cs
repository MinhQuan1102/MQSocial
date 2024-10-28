namespace API.DTO.Comment;

public class CommentDto
{
    public int? CommentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TotalReactions { get; set; }
    public CommentUserDto User { get; set; }
    public int? RepliedCommentId { get; set; }
    public CommentDto RepliedComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}