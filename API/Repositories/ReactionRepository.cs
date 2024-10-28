using API.Data;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class ReactionRepository(DataContext context, IMapper mapper): IReactionRepository
{
    public void AddReaction(Reaction reaction)
    {
        context.Reactions.Add(reaction);
    }

    public async Task<Reaction?> GetReactionByUserIdAndPostId(int userId, int postId)
    {
        return await context.Reactions.Where(x => x.UserId == userId && x.PostId == postId).FirstOrDefaultAsync();
    }
    
    public async Task<Reaction?> GetReactionByUserIdAndCommentId(int userId, int commentId)
    {
        return await context.Reactions.Where(x => x.UserId == userId && x.CommentId == commentId).FirstOrDefaultAsync();
    }
}