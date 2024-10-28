using System.Net;
using API.DTO;
using API.DTO.Comment;
using API.DTO.Reaction;
using API.Entities;
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
    public class CommentsController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CommentDto>>> CreateComment(CreateUpdateCommentDto createCommentDto)
        {
            var userId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserById(userId);
            if (user == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            if (createCommentDto.PostId == null)
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null));
            }

            var comment = new Comment
            {
                PostId = createCommentDto.PostId ?? 0,
                Content = createCommentDto.Content,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            
            unitOfWork.CommentRepository.CreateComment(comment);
            user.Comments.Add(comment);
            var post = await unitOfWork.PostRepository.GetPostById(createCommentDto.PostId ?? 0);
            if (post == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Post not found", null));
            }
            post.Comments.Add(comment);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Fail to create comment", null));
            }
            
            var commentDto = mapper.Map<CommentDto>(comment);

            return Ok(
                new ApiResponse<CommentDto>((int)HttpStatusCode.OK, "Create comment successfully", commentDto)
            );
        }
        
        [HttpPut("react/{id}")]
        public async Task<ActionResult<ApiResponse<string>>> ReactComment([FromRoute] int id, [FromBody] ReactPostDto reactCommentDto)
        {
            var userId = User.GetUserId();
            var comment = await unitOfWork.CommentRepository.GetCommentById(id);
            if (comment == null) {
                return NotFound(
                    new ApiResponse<string>((int)HttpStatusCode.NotFound, "Comment not found", null)
                );
            }
            var reaction = await unitOfWork.ReactionRepository.GetReactionByUserIdAndCommentId(userId, id);
            if (reaction != null)
            {
                reaction.Type = reactCommentDto.Type;
            }
            else
            {
                var newReaction = new Reaction
                {
                    CommentId = id,
                    UserId = userId,
                    Type = reactCommentDto.Type
                };
                unitOfWork.ReactionRepository.AddReaction(newReaction);
            }
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Fail to react comment", null));
            }
            return Ok(new ApiResponse<string>((int)HttpStatusCode.OK, "React comment successfully", null));
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateComment([FromRoute] int id, CreateUpdateCommentDto updateCommentDto)
        {
            var userId = User.GetUserId();
            var comment = await unitOfWork.CommentRepository.GetCommentById(id);
            var commentDto = await unitOfWork.CommentRepository.GetCommentDtoById(id);
            if (commentDto == null || comment == null) 
            {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Comment not found", null));
            }
            if (commentDto.User.Id != userId)
            {
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "You are not allowed to edit other user's comments", null));
            }

            mapper.Map(updateCommentDto, comment);
            comment.IsEdited = true;
            comment.UpdatedAt = DateTime.Now;
            
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to update comment", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Update comment successfully", null));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteComment([FromRoute] int id) {
            var userId = User.GetUserId();
            var comment = await unitOfWork.CommentRepository.GetCommentById(id);
            var commentDto = await unitOfWork.CommentRepository.GetCommentDtoById(id);

            if (comment == null || commentDto == null) {
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "Comment not found", null));
            }
            if (commentDto.User.Id != userId)
            {
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "You are not allowed to delete other user's comments", null));
            }
            unitOfWork.CommentRepository.DeleteComment(comment);
            if (!await unitOfWork.Complete()) {
                return BadRequest(new ApiResponse<string>((int) HttpStatusCode.BadRequest, "Fail to delete comment", null));
            }
            return Ok(new ApiResponse<string>((int) HttpStatusCode.OK, "Delete comment successfully", null));

        }
    }
}

