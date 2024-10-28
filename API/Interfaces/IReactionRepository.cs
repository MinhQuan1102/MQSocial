using API.Entities;

namespace API.Interfaces;

public interface IReactionRepository
{
    void AddReaction(Reaction reaction);
    Task<Reaction?> GetReactionByUserIdAndPostId(int userId, int postId);
    Task<Reaction?> GetReactionByUserIdAndCommentId(int userId, int commentId);
}