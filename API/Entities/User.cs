using System;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? AvatarId { get; set; }
    public virtual Image? Avatar { get; set; } 
    public int? CoverImageId { get; set; }
    public virtual Image? CoverImage { get; set; } 
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Friendship> FriendsInitiated { get; set; } = new List<Friendship>();
    public ICollection<Friendship> FriendsReceived { get; set; } = new List<Friendship>();
    public ICollection<Following> FollowingUsers { get; set; } = new List<Following>();
    public ICollection<Following> FollowedUsers { get; set; } = new List<Following>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    // Join Column
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public IList<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

}
