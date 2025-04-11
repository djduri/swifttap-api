using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLinkAddOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "Cards",
                table: "Links",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                schema: "Cards",
                table: "Links");
        }
    }
}
