using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.Auth;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public bool Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }    
    public string? Country { get; set; }
    [Required]
    public DateOnly DateOfBirth { get; set; }
}
