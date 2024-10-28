using System.Net;
using API.DTO;
using API.DTO.User;
using API.Entities;
using API.Enums;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper)
        : BaseApiController
    {
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserProfile () {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }
            var userDto = mapper.Map<UserDto>(user);
            return Ok(new ApiResponse<UserDto>((int)HttpStatusCode.OK, "Get user profile successfully", userDto));
        }

        [HttpPost("change-avatar-cover-image")]
        public async Task<ActionResult<ApiResponse<string>>> ChangeAvatar([FromForm] ChangeImageDto changeImageDto) {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            var result = await photoService.AddPhotoAsync(changeImageDto.File);
            if (result.Error != null) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, result.Error.Message, null));
            }
            var image = new Image {
                Url = result.SecureUrl.AbsoluteUri,
                Path = result.PublicId
            };
            user.Images.Add(image);
            if (changeImageDto.Type == (int)EChangeUserImage.ChangeAvatar) {
                user.Avatar = image;
            } else if (changeImageDto.Type == (int)EChangeUserImage.ChangeCoverImage) {
                user.CoverImage = image;
            }
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Image added to Cloudinary but fail to save to change user's avatar", null));
            }
            return Ok(new ApiResponse<UserDto>((int)HttpStatusCode.OK, $"Change {(changeImageDto.Type == (int)EChangeUserImage.ChangeAvatar ? "avatar" : "cover image")} successfully", null));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById([FromRoute] int id) {
            var user = await unitOfWork.UserRepository.GetUserById(id);
            if (user == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }
            var userDto = mapper.Map<UserDto>(user);
            return Ok(
                new ApiResponse<UserDto>((int)HttpStatusCode.OK, "Get user successfully", userDto)
            );
        }

        [HttpGet] 
        public async Task<ActionResult<ApiResponse<UserDto>>> GetAllUsers([FromQuery] PaginationParams paginationParams, string searchQuery = "") {
            var users = await unitOfWork.UserRepository.GetAllUsers(paginationParams, searchQuery);
            foreach(var user in users.Items) {
                user.Age = user.DateOfBirth.CalculateAge();
            }
            return Ok(
                new ApiResponse<PagedList<UserDto>>(
                    (int)HttpStatusCode.OK, 
                    "Get user list successfully", 
                    users
                )
            );
        }

        [HttpGet("friend-list")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetFriendList(
            [FromQuery] PaginationParams paginationParams)
        {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }

            var friendList = await unitOfWork.UserRepository.GetFriendList(paginationParams, userId);
            foreach (var friend in friendList.Items)
            {
                friend.MutualFriendsCount = await unitOfWork.UserRepository.GetMutualFriendsCount(userId, friend.Id);
            }
            return Ok(
                new ApiResponse<PagedList<UserDto>>(
                    (int)HttpStatusCode.OK, 
                    "Get friend request list successfully", 
                    friendList
                )
            );
        }

        [HttpGet("friend-requests")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetFriendRequestList([FromQuery] PaginationParams paginationParams)
        {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }

            var friendRequests = await unitOfWork.UserRepository.GetFriendRequests(paginationParams, userId);
            foreach (var friend in friendRequests.Items)
            {
                friend.MutualFriendsCount = await unitOfWork.UserRepository.GetMutualFriendsCount(userId, friend.Id);
            }
            return Ok(
                new ApiResponse<PagedList<UserDto>>(
                    (int)HttpStatusCode.OK, 
                    "Get friend request list successfully", 
                    friendRequests
                )
            );
        }

        [HttpPost("request-add-friend")]
        public async Task<ActionResult<ApiResponse<string>>> RequestAddFriend(FriendRequestDto friendRequestDto)
        {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            var userRequest = await unitOfWork.UserRepository.GetUserById(friendRequestDto.TargetUserId);
            if (userRequest == null || user == null)
            {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }

            if (friendRequestDto.TargetUserId == userId)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You cannot send friend request to yourself", null));
            }

            var existingFriendship =
                await unitOfWork.UserRepository.GetFriendshipByUserId(friendRequestDto.TargetUserId, userId);

            if (existingFriendship != null)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You cannot send friend request to this user", null));
            }

            var following = new Following
            {
                FollowingUserId = userId,
                FollowedUserId = friendRequestDto.TargetUserId,
                Status = (int) EFollowingStatus.Following
            };

            var friendship = new Friendship
            {
                User1Id = userId,
                User2Id = friendRequestDto.TargetUserId,
                Status = (int)EFriendStatus.Pending
            };
            
            var friendship2 = new Friendship
            {
                User1Id = friendRequestDto.TargetUserId,
                User2Id = userId,
                Status = (int)EFriendStatus.Unfriend
            };
            
            var existingFollowing = await unitOfWork.UserRepository.GetFollowingByUserId(friendRequestDto.TargetUserId, userId);
            if (existingFollowing == null)
            {
                unitOfWork.UserRepository.AddFollowing(following);
            }
            unitOfWork.UserRepository.AddFriendship(friendship);
            unitOfWork.UserRepository.AddFriendship(friendship2);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to send friend request", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Send friend request successfully", null));
        }

        [HttpPut("response-add-friend/{friendId}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> ResponseAddFriend([FromRoute] int friendId, [FromBody] FriendResponseDto friendResponseDto)
        {
            var userId = User.GetUserId();
            var currentUser = await unitOfWork.UserRepository.GetUserById(userId);
            var userRequest = await unitOfWork.UserRepository.GetUserById(friendId);
            
            if (userRequest == null || currentUser == null)
            {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null)
                );
            }
            if (friendId == userId)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You cannot accept friend request of yourself", null));
            }
            // received
            var existingFriendship1 =
                await unitOfWork.UserRepository.GetFriendshipByUserId(friendId, userId);
            // initiated
            var existingFriendship2 =
                await unitOfWork.UserRepository.GetFriendshipByUserId(userId, friendId);
            if (existingFriendship1 != null && existingFriendship1.Status != (int) EFriendStatus.Pending)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "This user did not send friend request to you", null));
            }

            if (existingFriendship1 is { Status: (int) EFriendStatus.Friend })
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "This user is your friend already", null));
            }

            if (existingFriendship1 != null && existingFriendship2 != null)
            {
                // accept friend request
                if (friendResponseDto.Type == (int)ERespondInvitation.Accept)
                {
                    // change friendship status of both side to Friend
                    existingFriendship1.Status = (int) EFriendStatus.Friend;
                    existingFriendship1.UpdatedAt = DateTime.UtcNow;
                    existingFriendship2.Status = (int) EFriendStatus.Friend;
                    existingFriendship2.UpdatedAt = DateTime.UtcNow;

                    // change following status of current user to accepted friend
                    var existingFollowing = await unitOfWork.UserRepository.GetFollowingByUserId(friendId, userId);
                    // if current user has not followed accepted friend
                    if (existingFollowing == null)
                    {
                        var following = new Following
                        {
                            FollowingUserId = userId,
                            FollowedUserId = friendId,
                            Status = (int)EFollowingStatus.Following
                        };
                        unitOfWork.UserRepository.AddFollowing(following);
                    }
                    else
                    {
                        existingFriendship1.Status = (int)EFollowingStatus.Following;
                    }
                }
                else
                {
                    unitOfWork.UserRepository.RemoveFriendship(existingFriendship1);   
                    unitOfWork.UserRepository.RemoveFriendship(existingFriendship2);   
                }
            }

            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Failed to add friend request", null));
            }
            
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Accept friend request successfully", null));
        }

        [HttpDelete("delete-friend/{friendId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteFriend([FromRoute] int friendId)
        {
            var userId = User.GetUserId();
            if (friendId == userId)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You cannot delete yourself from your friend list", null));
            }
            var friendships = await unitOfWork.UserRepository.GetFriendshipByIds(friendId, userId);
            var followings = await unitOfWork.UserRepository.GetFollowingByIds(friendId, userId);
            
            if (friendships.Count == 0 || followings.Count == 0)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Relationship not found", null));
            }

            foreach (var friendship in friendships)
            {
                if (friendship.Status != (int)EFriendStatus.Friend)
                {
                    return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You and this user is not friend", null));
                }
            }
            foreach (var following in followings)
            {
                following.Status = (int) EFollowingStatus.Unfollow;
            }
            unitOfWork.UserRepository.RemoveRangeFriendship(friendships);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Failed to delete friend", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Delete friend successfully", null));
        }
        
        [HttpPut]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUserInfo([FromBody] UpdateUserDTO updateUserDto) {
            var userId = User.GetUserId();
            var currentUser = await unitOfWork.UserRepository.GetUserById(userId);
            if (currentUser == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            mapper.Map(updateUserDto, currentUser);
            currentUser.UserName = updateUserDto.FirstName + " " + updateUserDto.LastName;
            var userDto = mapper.Map<UserDto>(currentUser);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to update user", null));
            }
            return Ok(new ApiResponse<UserDto>((int) HttpStatusCode.OK, "Update profile successfully", userDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser([FromRoute] int id) {
            var user = await unitOfWork.UserRepository.GetUserById(id);
            if (user == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            unitOfWork.UserRepository.DeleteUser(user);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to delete user", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Delete user successfully", null));

        }
    }
}
