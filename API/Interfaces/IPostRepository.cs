using API.DTO.Post;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface IPostRepository
{
    void CreatePost(Post post);
    Task<Post?> GetPostById(int postId);
    Task<PagedList<PostDto>> GetAllPosts(PaginationParams paginationParams);
    Task<PagedList<PostDto>> GetAllPostsOfUser(PaginationParams paginationParams, int userId);
    void DeletePost(Post post);
}