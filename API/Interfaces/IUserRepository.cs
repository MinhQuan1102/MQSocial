using API.DTO.User;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<PagedList<UserDto>> GetAllUsers(PaginationParams paginationParams, string searchQuery = "");
    Task<List<int>> GetExistUserIds(List<int> userIds);
    Task<PagedList<UserDto>> GetFriendList(PaginationParams paginationParams, int userId);
    Task<int> GetMutualFriendsCount(int user1Id, int user2Id);
    Task<PagedList<UserDto>> GetFriendRequests(PaginationParams paginationParams, int userId);
    Task<List<Following>> GetFollowingByIds(int followedUserId, int followingUserId);
    Task<Following?> GetFollowingByUserId(int followedUserId, int followingUserId);
    void AddFollowing(Following following);
    void RemoveFollowing(Following following);
    void RemoveRangeFollowing(IEnumerable<Following> followings);
    Task<List<Friendship>> GetFriendshipByIds(int user1Id, int user2Id);
    Task<Friendship?> GetFriendshipByUserId(int user1Id, int user2Id);
    
    void AddFriendship(Friendship friendship);
    void RemoveFriendship(Friendship friendship);
    void RemoveRangeFriendship(IEnumerable<Friendship> friendships);
    void DeleteUser(User user);
}   
