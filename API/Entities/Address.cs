using System;

namespace API.Entities;

public class Address
{
    public int AddressId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
}
