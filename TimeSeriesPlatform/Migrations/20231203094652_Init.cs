using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Iiroki.TimeSeriesPlatform.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tsp");

            migrationBuilder.CreateTable(
                name: "Integration",
                schema: "tsp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "tsp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                schema: "tsp",
                columns: table => new
                {
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Measurement_Integration_IntegrationId",
                        column: x => x.IntegrationId,
                        principalSchema: "tsp",
                        principalTable: "Integration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurement_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "tsp",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Integration_Slug",
                schema: "tsp",
                table: "Integration",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_IntegrationId",
                schema: "tsp",
                table: "Measurement",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_TagId",
                schema: "tsp",
                table: "Measurement",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_Timestamp_IntegrationId_TagId",
                schema: "tsp",
                table: "Measurement",
                columns: new[] { "Timestamp", "IntegrationId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Slug",
                schema: "tsp",
                table: "Tag",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurement",
                schema: "tsp");

            migrationBuilder.DropTable(
                name: "Integration",
                schema: "tsp");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "tsp");
        }
    }
}
