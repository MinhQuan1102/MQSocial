using System.Net;
using API.DTO;
using API.DTO.Comment;
using API.DTO.Post;
using API.DTO.Reaction;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PostDto>>> CreatePost(CreateUpdatePostDto createPostDto)
        {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }

            if (createPostDto.GroupId != null)
            {
                var group = await unitOfWork.GroupRepository.GetGroupById(createPostDto.GroupId.Value);
                if (group == null) {
                    return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Group not found", null));
                }
            }

            if (createPostDto.SharedPostId != null)
            {
                var sharedPost = await unitOfWork.PostRepository.GetPostById(createPostDto.SharedPostId.Value);
                if (sharedPost == null)
                {
                    return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Shared post not found", null));
                }
            }
            
            var files = createPostDto.Files;
            var postFiles = new List<Image>();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var result = await photoService.AddPhotoAsync(file);
                    postFiles.Add(new Image
                    {
                        Url = result.SecureUrl.AbsoluteUri,
                        Path = result.PublicId
                    });
                }
            }
            foreach (var image in postFiles)
            {
                user.Images.Add(image);
            }

            var post = new Post
            {
                Content = createPostDto.Content ?? "",
                UserId = userId,
                GroupId = createPostDto.GroupId,
                ParentPostId = createPostDto.SharedPostId,
                Images = postFiles,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            
            unitOfWork.PostRepository.CreatePost(post);
            user.Posts.Add(post);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Fail to create post", null));
            }
            var postDto = mapper.Map<PostDto>(post);

            return Ok(
                new ApiResponse<PostDto>((int)HttpStatusCode.OK, "Create post successfully", postDto)
            );
        }

        [HttpPut("react/{id}")]
        public async Task<ActionResult<ApiResponse<string>>> ReactPost([FromRoute] int id, [FromBody] ReactPostDto reactPostDto)
        {
            var userId = User.GetUserId();
            var post = await unitOfWork.PostRepository.GetPostById(id);
            if (post == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null)
                );
            }
            var reaction = await unitOfWork.ReactionRepository.GetReactionByUserIdAndPostId(userId, id);
            if (reaction != null)
            {
                reaction.Type = reactPostDto.Type;
            }
            else
            {
                var newReaction = new Reaction
                {
                    PostId = id,
                    UserId = userId,
                    Type = reactPostDto.Type
                };
                unitOfWork.ReactionRepository.AddReaction(newReaction);
            }
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Fail to react post", null));
            }
            return Ok(new ApiResponse<string>((int)HttpStatusCode.OK, "React post successfully", null));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PostDto>>> GetPostById([FromRoute] int id)
        {
            var post = await unitOfWork.PostRepository.GetPostById(postId: id);
            if (post == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null)
                );
            }

            var postDto = mapper.Map<PostDto>(post);
            postDto.TotalReactions = post.Reactions.Count;
            postDto.TotalComments = post.Comments.Count;
            return Ok(
                new ApiResponse<PostDto>((int)HttpStatusCode.OK, "Get post successfully", postDto)
            );
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<ApiResponse<List<PostDto>>>> GetPostsOfUser([FromRoute] int userId,
            [FromQuery] PaginationParams paginationParams)
        {
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }

            var posts = await unitOfWork.PostRepository.GetAllPostsOfUser(paginationParams, userId);
            return Ok(
                new ApiResponse<PagedList<PostDto>>(
                    (int)HttpStatusCode.OK, 
                    $"Get post list of {user.UserName} successfully", 
                    posts
                )
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdatePost([FromRoute] int id, CreateUpdatePostDto updatePostDto)
        {
            var userId = User.GetUserId();
            var post = await unitOfWork.PostRepository.GetPostById(id);
            if (post == null) 
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null));
            }
            if (post.User.Id != userId)
            {
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "You are not allowed to edit other user's posts", null));
            }

            mapper.Map(updatePostDto, post);
            post.IsEdited = true;
            post.UpdatedAt = DateTime.Now;
            
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to update post", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Update post successfully", null));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeletePost([FromRoute] int id) {
            var userId = User.GetUserId();
            var post = await unitOfWork.PostRepository.GetPostById(id);

            if (post == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null));
            }
            if (post.User.Id != userId)
            {
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "You are not allowed to delete other user's posts", null));
            }
            unitOfWork.PostRepository.DeletePost(post);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to delete post", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Delete post successfully", null));

        }
    }
}

