using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updateGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "JoinedAt",
                table: "UserGroups",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveNotification",
                table: "UserGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Groups",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoverImageId",
                table: "Groups",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AvatarId",
                table: "Groups",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CoverImageId",
                table: "Groups",
                column: "CoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Images_AvatarId",
                table: "Groups",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Images_CoverImageId",
                table: "Groups",
                column: "CoverImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Images_AvatarId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Images_CoverImageId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_AvatarId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CoverImageId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "ReceiveNotification",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");
        }
    }
}
