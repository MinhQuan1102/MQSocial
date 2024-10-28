using System;

namespace API.Entities;

public class UserGroup
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;

    public int Role { get; set; }
    public bool ReceiveNotification { get; set; } = true;
    public DateOnly JoinedAt { get; set; }
}
