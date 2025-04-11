using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCardAddPhoneAndEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Cards",
                table: "Cards",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailShareable",
                schema: "Cards",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneNumberShareable",
                schema: "Cards",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Cards",
                table: "Cards",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Cards",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsEmailShareable",
                schema: "Cards",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsPhoneNumberShareable",
                schema: "Cards",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Cards",
                table: "Cards");
        }
    }
}
