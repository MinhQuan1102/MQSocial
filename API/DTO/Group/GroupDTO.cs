namespace API.DTO.Group;

public class GroupDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public string? CoverImage { get; set; }
    public int TotalMembers { get; set; }
}