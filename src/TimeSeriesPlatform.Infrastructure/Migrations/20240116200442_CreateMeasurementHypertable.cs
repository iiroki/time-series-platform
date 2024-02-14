using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iiroki.TimeSeriesPlatform.InfrastructureMigrations
{
    /// <inheritdoc />
    public partial class CreateMeasurementHypertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var table = TspDbContext.ToTableName(nameof(TspDbContext.Measurement));

            migrationBuilder.Sql(
                $"""
                SELECT create_hypertable(
                    '{table}',
                    by_range('{nameof(MeasurementEntity.Timestamp)}', INTERVAL '1 day')
                );
                """
            );

            migrationBuilder.Sql(
                $"""
                SELECT add_dimension(
                    '{table}',
                    by_hash('{nameof(MeasurementEntity.IntegrationId)}', 2)
                );
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
