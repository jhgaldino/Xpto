using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XptoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesUrlToMenuItems3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagemUrl",
                value: "/static/images/coffee.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagemUrl",
                value: "/static/images/cafe.jpg");
        }
    }
}
