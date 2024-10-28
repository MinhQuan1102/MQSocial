namespace API.DTO.Group;

public class CreateUpdateGroupDTO
{
    public string GroupName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public string? CoverImage { get; set; }
}