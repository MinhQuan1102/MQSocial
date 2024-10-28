using API.Entities;

namespace API.Interfaces;

public interface IGroupRepository
{
    void CreateGroup(Group group);
    Task<Group?> GetGroupById(int id);
}