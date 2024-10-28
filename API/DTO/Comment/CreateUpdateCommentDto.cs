namespace API.DTO.Comment;

public class CreateUpdateCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int? PostId { get; set; }
    public int? RepliedCommentId { get; set; }
}