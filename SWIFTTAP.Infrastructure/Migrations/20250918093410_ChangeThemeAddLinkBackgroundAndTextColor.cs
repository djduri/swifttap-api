using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeThemeAddLinkBackgroundAndTextColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkBackgroundColor",
                schema: "Cards",
                table: "Themes",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkTextColor",
                schema: "Cards",
                table: "Themes",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkBackgroundColor",
                schema: "Cards",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "LinkTextColor",
                schema: "Cards",
                table: "Themes");
        }
    }
}
