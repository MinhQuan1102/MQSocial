using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Post
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public string? Content { get; set; } = string.Empty;


    public bool IsEdited { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Post> ChildPosts { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public int? ParentPostId { get; set; }
    public virtual Post ParentPost { get; set; } = null!;

    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public int? GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;



}