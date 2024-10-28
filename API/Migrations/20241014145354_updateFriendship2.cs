using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updateFriendship2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Following_AspNetUsers_FollowedUserId",
                table: "Following");

            migrationBuilder.DropForeignKey(
                name: "FK_Following_AspNetUsers_FollowingUserId",
                table: "Following");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Following",
                table: "Following");

            migrationBuilder.RenameTable(
                name: "Following",
                newName: "Followings");

            migrationBuilder.RenameIndex(
                name: "IX_Following_FollowingUserId",
                table: "Followings",
                newName: "IX_Followings_FollowingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Following_FollowedUserId",
                table: "Followings",
                newName: "IX_Followings_FollowedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followings",
                table: "Followings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_FollowedUserId",
                table: "Followings",
                column: "FollowedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_FollowingUserId",
                table: "Followings",
                column: "FollowingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_FollowedUserId",
                table: "Followings");

            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_FollowingUserId",
                table: "Followings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followings",
                table: "Followings");

            migrationBuilder.RenameTable(
                name: "Followings",
                newName: "Following");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_FollowingUserId",
                table: "Following",
                newName: "IX_Following_FollowingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_FollowedUserId",
                table: "Following",
                newName: "IX_Following_FollowedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Following",
                table: "Following",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Following_AspNetUsers_FollowedUserId",
                table: "Following",
                column: "FollowedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Following_AspNetUsers_FollowingUserId",
                table: "Following",
                column: "FollowingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
