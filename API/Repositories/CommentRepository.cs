using API.Data;
using API.DTO.Comment;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class CommentRepository(DataContext context, IMapper mapper): ICommentRepository
{
    public void CreateComment(Comment comment)
    {
        context.Comments.Add(comment);
    }

    public async Task<CommentDto?> GetCommentDtoById(int commentId)
    {
        return await context.Comments.Where(x => x.Id == commentId)
            .Select(x => new CommentDto
            {
                CommentId = x.Id,
                Content = x.Content,
                TotalReactions = x.TotalReactions,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                User = new CommentUserDto
                {
                    Id = x.User.Id,
                    FullName= x.User.FullName,
                    Avatar = x.User.Avatar != null ? x.User.Avatar.Url : "",
                    DateOfBirth = x.User.DateOfBirth,
                    Gender = x.User.Gender == true ? "Male" : "Female"
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Comment?> GetCommentById(int commentId)
    {
        return await context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
    }

    public async Task<PagedList<CommentDto>> GetCommentsOfPost(PaginationParams paginationParams, int postId)
    {
        var comments = context.Comments.Where(x => x.PostId == postId).AsQueryable();
        return await PagedList<CommentDto>.CreateAsync(comments.AsNoTracking().ProjectTo<CommentDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
    }

    public void DeleteComment(Comment comment)
    {
        context.Comments.Remove(comment);
    }
}