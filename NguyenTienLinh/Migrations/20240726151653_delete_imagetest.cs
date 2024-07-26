using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenTienLinh.Migrations
{
    /// <inheritdoc />
    public partial class delete_imagetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageTest",
                table: "BackGround");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageTest",
                table: "BackGround",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
