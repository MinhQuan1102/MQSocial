using API.DTO.Comment;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface ICommentRepository
{
    void CreateComment(Comment comment);
    Task<CommentDto?> GetCommentDtoById(int commentId);
    Task<Comment?> GetCommentById(int commentId);
    Task<PagedList<CommentDto>> GetCommentsOfPost(PaginationParams paginationParams, int userId);
    void DeleteComment(Comment comment);
}