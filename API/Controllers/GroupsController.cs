using System.Net;
using API.DTO;
using API.DTO.Comment;
using API.DTO.Group;
using API.Entities;
using API.Enums;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup(CreateUpdateGroupDTO createGroupDto)
        {
            var userId = User.GetUserId();
            var currentUser = await unitOfWork.UserRepository.GetUserById(userId);
            if (currentUser == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            var newGroup = new Group
            {
                GroupName = createGroupDto.GroupName,
                Description = createGroupDto.Description,
                AvatarId = null,
                Avatar = null,
                CoverImageId = null,
                CoverImage = null,
            };
            
            unitOfWork.GroupRepository.CreateGroup(newGroup);
            var userGroup = new UserGroup()
            {
                UserId = userId,
                GroupId = newGroup.Id,
                JoinedAt = DateOnly.FromDateTime(DateTime.Now),
                ReceiveNotification = true,
                Role = (int) EGroupRole.Admin,
            };
            currentUser.UserGroups.Add(userGroup);
            newGroup.UserGroups.Add(userGroup);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to create group", null));
            }
            var groupDto = mapper.Map<GroupDto>(newGroup);
            return Ok(new ApiResponse<GroupDto>((int) HttpStatusCode.OK, "Create group successfully", groupDto));
        }

        [HttpPost("add-members")]
        public async Task<ActionResult<string>> AddMembers(AddGroupMemberDTO addGroupMemberDto) 
        {
            var group = await unitOfWork.GroupRepository.GetGroupById(addGroupMemberDto.GroupId);
            if (group == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Group not found", null));
            }

            if (addGroupMemberDto.UserIds.Contains(User.GetUserId()))
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You are already in the group", null));
            }

            var userIds = await unitOfWork.UserRepository.GetExistUserIds(addGroupMemberDto.UserIds);
            var userGroups = new List<UserGroup>();
            foreach (int id in userIds) 
            {
                var userGroup = new UserGroup()
                {
                    UserId = id,
                    GroupId = addGroupMemberDto.GroupId,
                    Role = (int) EGroupRole.InvitedMember,
                };
                userGroups.Add(userGroup);
            }
            unitOfWork.UserGroupRepository.AddRange(userGroups);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to add members to group", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Add user to group successfully", null));
        }

        [HttpGet("invitations")]
        public async Task<ActionResult<ApiResponse<List<GroupDto>>>> GetInvitations()
        {
            var userId = User.GetUserId();
            var groupInvitations = await unitOfWork.UserGroupRepository.GetInvitations(userId);
            var groupInvitationDto = groupInvitations.Select(x => new GroupDto
            {
                GroupId = x.Id,
                Avatar = x.Avatar?.Url,
                GroupName = x.GroupName,
                Description = x.Description,
            });
            return Ok(new ApiResponse<IEnumerable<GroupDto>>((int) HttpStatusCode.OK, "Get group invitation successfully", groupInvitationDto));
        }

        [HttpPut("invitations/respond/{groupId}")]
        public async Task<ActionResult<ApiResponse<string>>> RespondGroupInvitation([FromRoute] int groupId,
            [FromBody] RespondInvitationDto respondInvitationDto)
        {
            var userId = User.GetUserId();
            var userGroup = await unitOfWork.UserGroupRepository.Get(groupId, userId);
            if (userGroup == null)
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User or group not found", null));
            }
            
            if (new int[] { (int)EGroupRole.Admin, (int)EGroupRole.Member, (int)EGroupRole.Viewer }.Contains(
                    userGroup.Role))
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You are already in the group", null));
            }

            if (respondInvitationDto.Type == (int)ERespondInvitation.Accept)
            {
                userGroup.Role = (int)EGroupRole.Member;
                userGroup.JoinedAt = DateOnly.FromDateTime(DateTime.Now);
            }
            else if (respondInvitationDto.Type == (int)ERespondInvitation.Decline)
            {
                unitOfWork.UserGroupRepository.Remove(userGroup);
            }

            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, $"Fail to {(respondInvitationDto.Type == (int) ERespondInvitation.Accept ? "accept" : "decline")} group invitation", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, $"{(respondInvitationDto.Type == (int) ERespondInvitation.Accept ? "Accept" : "Decline")} group invitation successfully", null));
        }

        [HttpPost("request-join")]
        public async Task<ActionResult<ApiResponse<string>>> RequestJoinGroup(
            [FromBody] RequestJoinGroupDto requestJoinGroupDto)
        {
            var userId = User.GetUserId();
            var groupId = requestJoinGroupDto.GroupId;
            var group = await unitOfWork.GroupRepository.GetGroupById(groupId);
            
            if (group == null)
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Group not found", null));
            }
            
            if (await unitOfWork.UserGroupRepository.Get(groupId, userId) != null)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "You are already in the group or a pending member", null));
            }
            var userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = groupId,
                Role = (int)EGroupRole.RequestMember
            };
            unitOfWork.UserGroupRepository.Add(userGroup);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to send request to join group", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Send request to join group successfully", null));
        }

        [HttpDelete("{groupId}/leave")]
        public async Task<ActionResult<ApiResponse<string>>> LeaveGroup([FromRoute] int groupId)
        {
            var userId = User.GetUserId();
            var userGroup = await unitOfWork.UserGroupRepository.Get(groupId, userId);
            if (userGroup == null)
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User or group not found", null));
            }

            if (!new int[] { (int)EGroupRole.Admin, (int)EGroupRole.Member, (int)EGroupRole.Viewer }.Contains(
                    userGroup.Role))
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "You are not in the group", null));
            }
            unitOfWork.UserGroupRepository.Remove(userGroup);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, $"Fail to leave group", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, $"Leave group successfully", null));
        }
    }
}

