using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iiroki.TimeSeriesPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddMeasurementIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Measurement_TagId",
                schema: "tsp",
                table: "Measurement");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_TagId_LocationId_Timestamp",
                schema: "tsp",
                table: "Measurement",
                columns: new[] { "TagId", "LocationId", "Timestamp" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_TagId_Timestamp",
                schema: "tsp",
                table: "Measurement",
                columns: new[] { "TagId", "Timestamp" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_VersionTimestamp",
                schema: "tsp",
                table: "Measurement",
                column: "VersionTimestamp",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Measurement_TagId_LocationId_Timestamp",
                schema: "tsp",
                table: "Measurement");

            migrationBuilder.DropIndex(
                name: "IX_Measurement_TagId_Timestamp",
                schema: "tsp",
                table: "Measurement");

            migrationBuilder.DropIndex(
                name: "IX_Measurement_VersionTimestamp",
                schema: "tsp",
                table: "Measurement");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_TagId",
                schema: "tsp",
                table: "Measurement",
                column: "TagId");
        }
    }
}
