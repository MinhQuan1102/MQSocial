namespace API.DTO.Group;

public class AddGroupMemberDTO
{
    public int GroupId { get; set; }
    public List<int> UserIds { get; set; }
}