using System;

namespace API.Entities;

public class Group
{
    public int Id { get; set; }
    public string GroupName { get; set; }
    public string? Description { get; set; }
    public int? AvatarId { get; set; }
    public virtual Image? Avatar { get; set; } = null!;
    public int? CoverImageId { get; set; }
    public virtual Image? CoverImage { get; set; } = null!;
    public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
