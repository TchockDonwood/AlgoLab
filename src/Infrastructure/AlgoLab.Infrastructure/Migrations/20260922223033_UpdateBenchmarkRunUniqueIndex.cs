using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlgoLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBenchmarkRunUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_benchmark_runs_AlgorithmId_N",
                table: "benchmark_runs");

            migrationBuilder.CreateIndex(
                name: "IX_benchmark_runs_AlgorithmId_N_M",
                table: "benchmark_runs",
                columns: new[] { "AlgorithmId", "N", "M" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_benchmark_runs_AlgorithmId_N_M",
                table: "benchmark_runs");

            migrationBuilder.CreateIndex(
                name: "IX_benchmark_runs_AlgorithmId_N",
                table: "benchmark_runs",
                columns: new[] { "AlgorithmId", "N" },
                unique: true);
        }
    }
}
