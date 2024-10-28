namespace API.DTO.Post;

public class PostUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
}