using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLinkAddCustomLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasIcon",
                schema: "Cards",
                table: "Links",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LinkKind",
                schema: "Cards",
                table: "Links",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LinkIcons",
                schema: "Cards",
                columns: table => new
                {
                    LinkId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: true),
                    ContentType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkIcons", x => x.LinkId);
                    table.ForeignKey(
                        name: "FK_LinkIcons_Links_LinkId",
                        column: x => x.LinkId,
                        principalSchema: "Cards",
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkIcons",
                schema: "Cards");

            migrationBuilder.DropColumn(
                name: "HasIcon",
                schema: "Cards",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "LinkKind",
                schema: "Cards",
                table: "Links");
        }
    }
}
