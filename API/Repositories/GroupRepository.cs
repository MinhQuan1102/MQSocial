using API.Data;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class GroupRepository(DataContext context, IMapper mapper) : IGroupRepository
{
    public void CreateGroup(Group group)
    {
        context.Groups.Add(group);
    }

    public async Task<Group?> GetGroupById(int id)
    {
        return await context.Groups.FirstOrDefaultAsync(x => x.Id == id);
    }
}