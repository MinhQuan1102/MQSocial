using System;

namespace API.Interfaces;

public interface IUnitOfWork
{
    IAuthRepository AuthRepository { get; }
    IUserRepository UserRepository { get; } 
    IPostRepository PostRepository { get; }
    ICommentRepository CommentRepository { get; }
    IGroupRepository GroupRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IReactionRepository ReactionRepository { get; }

    Task<bool> Complete();

}
