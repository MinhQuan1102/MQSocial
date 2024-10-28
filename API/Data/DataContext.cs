using System;
using API.DTO.Comment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options)
    : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
{
    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public override DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Following> Followings { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // User
        builder.Entity<User>()
            .HasMany(u => u.FriendsInitiated)               
            .WithOne(f => f.User1)
            .HasForeignKey(f => f.User1Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<User>()
            .HasMany(u => u.FriendsReceived)               
            .WithOne(f => f.User2)
            .HasForeignKey(f => f.User2Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<User>()
            .HasMany<Following>(f => f.FollowedUsers)
            .WithOne(i => i.FollowedUser)
            .HasForeignKey(i => i.FollowedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<User>()
            .HasMany<Following>(f => f.FollowingUsers)
            .WithOne(i => i.FollowingUser)
            .HasForeignKey(i => i.FollowingUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // User Role
        builder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId});
        builder.Entity<UserRole>()
            .HasOne<User>(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<UserRole>()
            .HasOne<Role>(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // User Group
        builder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId});
        builder.Entity<UserGroup>()
            .HasOne<User>(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);

        builder.Entity<UserGroup>()
            .HasOne<Group>(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);


        // Image
        builder.Entity<User>()
            .HasMany<Image>(u => u.Images)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<User>()
            .HasOne(u => u.Avatar)
            .WithMany()
            .HasForeignKey(u => u.AvatarId) 
            .OnDelete(DeleteBehavior.SetNull); 

        builder.Entity<User>()
            .HasOne(u => u.CoverImage)
            .WithMany()
            .HasForeignKey(u => u.CoverImageId) 
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Post>()
            .HasMany<Image>(p => p.Images)
            .WithOne(i => i.Post)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post
        builder.Entity<User>()
            .HasMany<Post>(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasMany(p => p.ChildPosts)
            .WithOne(p => p.ParentPost)
            .HasForeignKey(p => p.ParentPostId);

        builder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasMany(p => p.Reactions)
            .WithOne(r => r.Post)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment
        builder.Entity<User>()
            .HasMany<Comment>(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Comment>()
            .HasMany(c => c.ChildComments)
            .WithOne(c => c.ParentComment)
            .HasForeignKey(c => c.ParentCommentId);
        
        builder.Entity<Comment>()
            .HasMany(p => p.Reactions)
            .WithOne(r => r.Comment)
            .HasForeignKey(r => r.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Reaction
        builder.Entity<User>()
            .HasMany<Reaction>(u => u.Reactions)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
