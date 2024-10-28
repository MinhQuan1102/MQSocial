using System;
using API.DTO.Auth;
using API.DTO.Comment;
using API.DTO.Group;
using API.DTO.Post;
using API.DTO.Reaction;
using API.DTO.User;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Auth
        CreateMap<User, RegisterRequestDto>().ReverseMap();
        CreateMap<User, LoginRequestDto>().ReverseMap();

        // User
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url))
            .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.CoverImage.Url)).ReverseMap();
        CreateMap<User, UpdateUserDTO>().IgnoreNullValues();
        CreateMap<UpdateUserDTO, User>().IgnoreNullValues();
        CreateMap<Following, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FollowedUser.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.FollowedUser.Avatar != null ? src.FollowedUser.Avatar.Url : ""))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.FollowedUser.DateOfBirth))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FollowedUser.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.FollowedUser.Gender == true ? "Male" : "Female"))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.FollowedUser.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.FollowedUser.City))
            .ReverseMap();
        CreateMap<Friendship, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User1.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User1.Avatar != null ? src.User1.Avatar.Url : ""))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.User1.DateOfBirth))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User1.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User1.Gender == true ? "Male" : "Female"))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.User1.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.User1.City))
            .ReverseMap();
        CreateMap<Friendship, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User2.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User2.Avatar != null ? src.User2.Avatar.Url : ""))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.User2.DateOfBirth))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User2.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User2.Gender == true ? "Male" : "Female"))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.User2.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.User2.City))
            .ReverseMap();
        CreateMap<UserDto, UserDto>();

        // Post
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.User, opt =>
                opt.MapFrom(src => new PostUserDto
                {
                    Id = src.User.Id,
                    FullName = src.User.FullName,
                    Gender = src.User.Gender == true ? "Male" : "Female",
                    DateOfBirth = src.User.DateOfBirth,
                    Avatar = src.User.Avatar != null ? src.User.Avatar.Url : "",
                }))
            .ForMember(dest => dest.ParentPost, opt => 
                opt.MapFrom(src => new PostDto
                {
                    Id = src.ParentPost.Id,
                    Content = src.ParentPost.Content ?? "",
                    User = new PostUserDto
                    {
                        Id = src.ParentPost.User.Id,
                        FullName = src.ParentPost.User.FullName,
                        Gender = src.ParentPost.User.Gender == true ? "Male" : "Female",
                        DateOfBirth = src.ParentPost.User.DateOfBirth,
                        Avatar = src.ParentPost.User.Avatar != null ? src.ParentPost.User.Avatar.Url : "",
                    },
                    Images = src.ParentPost.Images.Select(x => x.Url).ToList(),
                    CreatedAt = src.ParentPost.CreatedAt,
                    UpdatedAt = src.ParentPost.UpdatedAt
                }))
            .ForMember(dest => dest.Comments, opt =>
                opt.MapFrom(src => src.Comments.Select(c => new CommentDto
                {
                    CommentId = c.Id,
                    Content = c.Content,
                    TotalReactions = c.Reactions.Count,
                    User = new CommentUserDto
                    {
                        Id = c.User.Id,
                        FullName = c.User.FullName,
                        Avatar = c.User.Avatar != null ? c.User.Avatar.Url : "",
                        Gender = c.User.Gender == true ? "Male" : "Female",
                        DateOfBirth = c.User.DateOfBirth
                    },
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                }).ToList()))
            .ForMember(dest => dest.Reactions, opt => 
                opt.MapFrom(src => src.Reactions.Select(r => new ReactionDto
                {
                    Type = r.Type,
                    User = new ReactionUserDto
                    {
                        Id = r.User.Id,
                        UserName = r.User.FullName,
                        Avatar = r.User.Avatar != null ? r.User.Avatar.Url : "",
                        Gender = r.User.Gender == true ? "Male" : "Female",
                        DateOfBirth = r.User.DateOfBirth
                    }
                })))
            .ForMember(dest => dest.TotalReactions, opt => opt.MapFrom(src => src.Reactions.Count()))
            .ForMember(dest => dest.TotalComments, opt => opt.MapFrom(src => src.Comments.Count()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()))
            .IgnoreNullValues();
        CreateMap<User, PostUserDto>().ReverseMap();
        CreateMap<CreateUpdatePostDto, Post>().ReverseMap();
        
        // Comment
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
        CreateMap<User, CommentUserDto>().ReverseMap();
        CreateMap<CreateUpdateCommentDto, Comment>().ReverseMap();
        
        // Group
        CreateMap<Group, GroupDto>().ReverseMap();
        CreateMap<Group, CreateUpdateGroupDTO>().ReverseMap();
    }
}
