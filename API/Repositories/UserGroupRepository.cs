using API.Data;
using API.Entities;
using API.Enums;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserGroupRepository(DataContext context, IMapper mapper): IUserGroupRepository
{
    public void Add(UserGroup userGroup)
    {
        context.UserGroups.Add(userGroup);
    }
    
    public void AddRange(IEnumerable<UserGroup> userGroups)
    {
        context.UserGroups.AddRange(userGroups);
    }

    public async Task<UserGroup?> Get(int groupId, int userId)
    {
        return await context.UserGroups.FindAsync(userId, groupId);
    }

    public async Task<IEnumerable<Group>> GetInvitations(int userId)
    {
        return await context.UserGroups
            .Where(x => x.UserId == userId && x.Role == (int) EGroupRole.InvitedMember)
            .Select(x => x.Group).ToListAsync();
    }

    public void Remove(UserGroup userGroup)
    {
        context.UserGroups.Remove(userGroup);
    }
}