using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.Auth;

public class LoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
