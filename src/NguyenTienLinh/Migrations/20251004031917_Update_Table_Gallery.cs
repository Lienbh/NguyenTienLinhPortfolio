using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenTienLinh.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_Gallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "BannerDescription",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "BannerSubtitle",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "BannerTitle",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Galleries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "GalleryItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GalleryItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "GalleryItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerDescription",
                table: "Galleries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerSubtitle",
                table: "Galleries",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerTitle",
                table: "Galleries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Galleries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Galleries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Galleries",
                type: "datetime2",
                nullable: true);
        }
    }
}
