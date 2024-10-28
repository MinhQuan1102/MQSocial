namespace API.DTO.User;

public class ChangeImageDto
{
    public IFormFile File { get; set; }
    public int Type { get; set; }
}