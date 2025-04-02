using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeThemeAddTextDarkLightAndOthers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Background",
                schema: "Cards",
                table: "Themes",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextDark",
                schema: "Cards",
                table: "Themes",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextLight",
                schema: "Cards",
                table: "Themes",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextOnButtons",
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
                name: "Background",
                schema: "Cards",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "TextDark",
                schema: "Cards",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "TextLight",
                schema: "Cards",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "TextOnButtons",
                schema: "Cards",
                table: "Themes");
        }
    }
}
