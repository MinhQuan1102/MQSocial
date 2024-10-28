using System;
using API.Entities;

namespace API.DTO.User;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public string? CoverImage { get; set; }
    public int? Age { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public String? Country { get; set; }
    public string? City { get; set; }
    public DateTime LastActive { get; set; }
    public int? MutualFriendsCount { get; set; } = 0;
    public int? FollowerCount { get; set; } = 0;
    public int? FriendCount { get; set; } = 0;
}
