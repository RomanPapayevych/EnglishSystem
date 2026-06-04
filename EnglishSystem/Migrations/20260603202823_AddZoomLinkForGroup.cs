using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddZoomLinkForGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZoomLink",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoomLink",
                table: "Groups");
        }
    }
}
