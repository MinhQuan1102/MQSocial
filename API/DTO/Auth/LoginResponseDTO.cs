using System;

namespace API.DTO.Auth;

public class LoginResponseDto
{      
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Token { get; set; } = string.Empty;
}
