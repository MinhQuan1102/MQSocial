using System;
using API.Data;
using API.Interfaces;
using AutoMapper;

namespace API.Repositories;

public class UnitOfWork(DataContext context, IMapper mapper) : IUnitOfWork
{
    public IAuthRepository AuthRepository => new AuthRepository(context, mapper);

    public IUserRepository UserRepository => new UserRepository(context, mapper);
    public IPostRepository PostRepository => new PostRepository(context, mapper);
    public ICommentRepository CommentRepository => new CommentRepository(context, mapper);
    public IGroupRepository GroupRepository => new GroupRepository(context, mapper);
    public IUserGroupRepository UserGroupRepository => new UserGroupRepository(context, mapper);
    public IReactionRepository ReactionRepository => new ReactionRepository(context, mapper);
    public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

}
