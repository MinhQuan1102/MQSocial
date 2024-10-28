using System;

namespace API.DTO;

public class ImageDto
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
