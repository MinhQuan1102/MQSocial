namespace API.DTO.Post;

public class CreateUpdatePostDto
{
    public string? Content { get; set; } = string.Empty;
    public List<IFormFile>? Files { get; set; }
    public int? SharedPostId { get; set; }
    public int? GroupId { get; set; }
}