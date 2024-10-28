using API.Entities;

namespace API.Interfaces;

public interface IUserGroupRepository
{
    void Add(UserGroup userGroup);
    void AddRange(IEnumerable<UserGroup> userGroups);
    Task<UserGroup?> Get(int groupId, int userId);
    Task<IEnumerable<Group>> GetInvitations(int userId);
    void Remove(UserGroup userGroup);
}