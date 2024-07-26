using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenTienLinh.Migrations
{
    /// <inheritdoc />
    public partial class add_imgatest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageTest",
                table: "BackGround",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageTest",
                table: "BackGround");
        }
    }
}
