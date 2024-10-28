using System;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; } = new User();
    public Role Role { get; set; } = new Role();
}
