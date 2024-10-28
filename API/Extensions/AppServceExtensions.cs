using System;
using System.Text.Json.Serialization;
using API.Data;
using API.Helper;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class AppServceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) {
        services.AddDbContext<DataContext>(opt => {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddCors();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<LogUserActivity>();       

        // Repository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();

        return services;
    } 
}
