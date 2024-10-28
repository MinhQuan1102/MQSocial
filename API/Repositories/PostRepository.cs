using API.Data;
using API.DTO.Comment;
using API.DTO.Post;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class PostRepository(DataContext context, IMapper mapper): IPostRepository
{
    public void CreatePost(Post post)
    {
        context.Posts.Add(post);
    }
    public async Task<Post?> GetPostById(int postId)
    {
        return await context.Posts
            .Include(x => x.Comments)
            .Include(x => x.User).ThenInclude(x => x.Avatar)
            .Include(x => x.Reactions).ThenInclude(x => x.User).ThenInclude(x => x.Avatar)
            .Include(x => x.Images)
            .Include(x => x.ParentPost).ThenInclude(x => x.User).ThenInclude(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<PagedList<PostDto>> GetAllPosts(PaginationParams paginationParams)
    {
        var posts = context.Posts.AsQueryable();
        return await PagedList<PostDto>.CreateAsync(posts.AsNoTracking().ProjectTo<PostDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<PagedList<PostDto>> GetAllPostsOfUser(PaginationParams paginationParams, int userId)
    {
        var posts = context.Posts.Where(x => x.UserId == userId).AsQueryable();
        return await PagedList<PostDto>.CreateAsync(posts.AsNoTracking().ProjectTo<PostDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
    }

    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }
}