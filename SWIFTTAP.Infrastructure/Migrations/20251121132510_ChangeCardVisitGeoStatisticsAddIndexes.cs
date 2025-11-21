using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWIFTTAP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCardVisitGeoStatisticsAddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CardVisitGeoStatistics_CardId_Date",
                schema: "Statistics",
                table: "CardVisitGeoStatistics",
                columns: new[] { "CardId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_CardVisitGeoStatistics_City_CountryCode",
                schema: "Statistics",
                table: "CardVisitGeoStatistics",
                columns: new[] { "City", "CountryCode" });

            migrationBuilder.CreateIndex(
                name: "IX_CardVisitGeoStatistics_Date",
                schema: "Statistics",
                table: "CardVisitGeoStatistics",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardVisitGeoStatistics_CardId_Date",
                schema: "Statistics",
                table: "CardVisitGeoStatistics");

            migrationBuilder.DropIndex(
                name: "IX_CardVisitGeoStatistics_City_CountryCode",
                schema: "Statistics",
                table: "CardVisitGeoStatistics");

            migrationBuilder.DropIndex(
                name: "IX_CardVisitGeoStatistics_Date",
                schema: "Statistics",
                table: "CardVisitGeoStatistics");
        }
    }
}
