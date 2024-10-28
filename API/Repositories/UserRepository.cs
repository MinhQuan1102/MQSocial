using System;
using API.Data;
using API.DTO.User;
using API.Entities;
using API.Enums;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }

    public async Task<PagedList<UserDto>> GetAllUsers(PaginationParams paginationParams, string searchQuery = "")
    {
        var users = context.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            users = users.Where(x => x.FullName.Contains(searchQuery));
                
        }
        var userListDto = users.Select(x => new UserDto()
        {
            Id = x.Id,
            FullName = x.FullName,
            Avatar = x.Avatar != null ? x.Avatar.Url : "",
            CoverImage = x.CoverImage != null ? x.CoverImage.Url : "",
            Gender = x.Gender == true ? "Male" : "Female",
            DateOfBirth = x.DateOfBirth,
            Country = x.Country,
            City = x.City,
            LastActive = x.LastActive,
            FriendCount = context.Friendships
                .Count(f => f.User1Id == x.Id && f.Status == (int)EFriendStatus.Friend),
            FollowerCount = context.Followings
                .Count(f => f.FollowedUserId == x.Id && f.Status == (int)EFollowingStatus.Following),
        });
        var results = await PagedList<UserDto>.CreateAsync(userListDto.AsNoTracking().ProjectTo<UserDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        return results;
    }

    public async Task<User?> GetUserById(int id)
    {
        var user = await context.Users
            .Include(x => x.Avatar)
            .Include(x => x.CoverImage)
            .FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }
    
    public async Task<List<int>> GetExistUserIds(List<int> userIds)
    {
        return await context.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.Id).ToListAsync();
    }
    
    public async Task<List<Following>> GetFollowingByIds(int followedUserId, int followingUserId)
    {
        return await context.Followings
            .Where(x => (x.FollowedUserId == followedUserId && x.FollowingUserId == followingUserId) || (x.FollowingUserId == followedUserId && x.FollowedUserId == followingUserId))
            .ToListAsync();
    }
    
    public async Task<Following?> GetFollowingByUserId(int followedUserId, int followingUserId)
    {
        return await context.Followings.FirstOrDefaultAsync(x => x.FollowedUserId == followedUserId && x.FollowingUserId == followingUserId);
    }

    public void AddFollowing(Following following)
    {
        context.Followings.Add(following);
    }

    public void RemoveFollowing(Following following)
    {
        context.Followings.Remove(following);
    }
    
    public void RemoveRangeFollowing(IEnumerable<Following> followings)
    {
        context.Followings.RemoveRange(followings);
    }
    
    public async Task<List<Friendship>> GetFriendshipByIds(int user1Id, int user2Id)
    {
        return await context.Friendships.Where(x => 
            (x.User1Id == user1Id && x.User2Id == user2Id) || (x.User1Id == user2Id && x.User2Id == user1Id)).ToListAsync();
    }
    public async Task<Friendship?> GetFriendshipByUserId(int user1Id, int user2Id)
    {
        return await context.Friendships.FirstOrDefaultAsync(x => 
            x.User1Id == user1Id && x.User2Id == user2Id);
    }

    public void AddFriendship(Friendship friendship)
    {
        context.Friendships.Add(friendship);
    }
    
    public void RemoveFriendship(Friendship friendship)
    {
        context.Friendships.Remove(friendship);
    }
    
    public void RemoveRangeFriendship(IEnumerable<Friendship> friendships)
    {
        context.Friendships.RemoveRange(friendships);
    }

    public async Task<PagedList<UserDto>> GetFriendRequests(PaginationParams paginationParams, int userId)
    {
        var requests = context.Friendships
            .Where(f => f.User2Id == userId && f.Status == (int) EFriendStatus.Pending)
            .Select(x => new UserDto
            {
                Id = x.User1Id,
                FullName = x.User1.FullName,
                Avatar = x.User1.Avatar != null ? x.User1.Avatar.Url : "",
                Gender = x.User1.Gender == true ? "Male" : "Female",
                DateOfBirth = x.User1.DateOfBirth,
                Country = x.User1.Country,
                City = x.User1.City,
                LastActive = x.User1.LastActive,
                FriendCount = context.Friendships
                    .Count(f => f.User1Id == x.User2Id && f.Status == (int)EFriendStatus.Friend),
                FollowerCount = context.Followings
                    .Count(f => f.FollowedUserId == x.User2Id && f.Status == (int)EFollowingStatus.Following),
            })
            .AsQueryable();
        var results = await PagedList<UserDto>.CreateAsync(requests.AsNoTracking().ProjectTo<UserDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        return results;
    }

    public async Task<PagedList<UserDto>> GetFriendList(PaginationParams paginationParams, int currentUserId)
    {
        var friendList = context.Friendships
            .Where(x => x.User1Id == currentUserId && x.Status == (int) EFriendStatus.Friend)
            .Select(x => new UserDto
            {
                Id = x.User2Id,
                FullName = x.User2.FullName,
                Avatar = x.User2.Avatar != null ? x.User2.Avatar.Url : "",
                Gender = x.User2.Gender == true ? "Male" : "Female",
                DateOfBirth = x.User2.DateOfBirth,
                Country = x.User2.Country,
                City = x.User2.City,
                FriendCount = context.Friendships
                    .Count(f => f.User1Id == x.User2Id && f.Status == (int)EFriendStatus.Friend),
                FollowerCount = context.Followings
                    .Count(f => f.FollowedUserId == x.User2Id && f.Status == (int)EFollowingStatus.Following),
            })
            .AsQueryable();
        
        var results = await PagedList<UserDto>.CreateAsync(friendList.AsNoTracking().ProjectTo<UserDto>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        return results;
    }

    public async Task<int> GetMutualFriendsCount(int user1Id, int user2Id)
    {
        var user1Friends = await context.Friendships
            .Where(f => f.User1Id == user1Id && f.Status == (int) EFriendStatus.Friend)
            .Select(f => f.User2Id)
            .ToListAsync();

        var user2Friends = await context.Friendships
            .Where(f => f.User1Id == user2Id && f.Status == (int) EFriendStatus.Friend) 
            .Select(f => f.User2Id)
            .ToListAsync();

        return user1Friends.Intersect(user2Friends).Count();
    }
}
