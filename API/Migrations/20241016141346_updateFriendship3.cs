using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updateFriendship3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendsId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friendships",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "FriendsId",
                table: "Friendships",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                newName: "IX_Friendships_User2Id");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Friendships",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Friendships",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Friendships",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Friendships",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_User1Id",
                table: "Friendships",
                column: "User1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_User1Id",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "Friendships",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "Friendships",
                newName: "FriendsId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_User2Id",
                table: "Friendships",
                newName: "IX_Friendships_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                columns: new[] { "FriendsId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendsId",
                table: "Friendships",
                column: "FriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
