using System;

namespace API.DTO.User;

public class UpdateUserDTO
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public DateOnly? DateOfBirth { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; } 
}
