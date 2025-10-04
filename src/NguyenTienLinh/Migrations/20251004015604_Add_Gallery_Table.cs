using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenTienLinh.Migrations
{
    /// <inheritdoc />
    public partial class Add_Gallery_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    IdGallery = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BannerImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BannerImageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BannerTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BannerSubtitle = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BannerDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.IdGallery);
                });

            migrationBuilder.CreateTable(
                name: "GalleryItems",
                columns: table => new
                {
                    IdGalleryItem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGallery = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryItems", x => x.IdGalleryItem);
                    table.ForeignKey(
                        name: "FK_GalleryItems_Galleries_IdGallery",
                        column: x => x.IdGallery,
                        principalTable: "Galleries",
                        principalColumn: "IdGallery",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_IdGallery",
                table: "GalleryItems",
                column: "IdGallery");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryItems");

            migrationBuilder.DropTable(
                name: "Galleries");
        }
    }
}
