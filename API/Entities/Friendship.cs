namespace API.Entities;

public class Friendship
{
    public int Id { get; set; }
    public int User1Id { get; set; }
    public virtual User User1 { get; set; } = null!;
    public int User2Id { get; set; }
    public virtual User User2 { get; set; } = null!;
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}