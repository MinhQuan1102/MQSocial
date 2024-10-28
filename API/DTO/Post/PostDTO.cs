using API.DTO.Comment;
using API.DTO.Reaction;
using API.DTO.User;
using API.Entities;

namespace API.DTO.Post;

public class PostDto
{
    public int Id { get; set; }
    public int? GroupId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TotalReactions { get; set; }
    public int TotalComments { get; set; }
    public PostUserDto? User { get; set; }
    public PostDto? ParentPost { get; set; }
    public ICollection<CommentDto>? Comments { get; set; }
    public ICollection<ReactionDto>? Reactions { get; set; }
    public List<string>? Images { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}