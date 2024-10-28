using System;

namespace API.Entities;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; } 
    public string Path { get; set; } 
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int? PostId { get; set; }
    public virtual Post? Post { get; set; } = null!;
}
