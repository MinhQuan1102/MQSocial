using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updateFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Following",
                table: "Following");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Following",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Following",
                table: "Following",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Following_FollowedUserId",
                table: "Following",
                column: "FollowedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Following",
                table: "Following");

            migrationBuilder.DropIndex(
                name: "IX_Following_FollowedUserId",
                table: "Following");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Following");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Following",
                table: "Following",
                columns: new[] { "FollowedUserId", "FollowingUserId" });
        }
    }
}
