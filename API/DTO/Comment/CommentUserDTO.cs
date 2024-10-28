namespace API.DTO.Comment;

public class CommentUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
}