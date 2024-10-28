namespace API.DTO.Reaction;

public class ReactionUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string? Avatar { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
}