using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlgoLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "algorithms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_algorithms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "benchmark_runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlgorithmId = table.Column<Guid>(type: "uuid", nullable: false),
                    N = table.Column<int>(type: "integer", nullable: false),
                    ExecutionTimeMs = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benchmark_runs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_benchmark_runs_algorithms_AlgorithmId",
                        column: x => x.AlgorithmId,
                        principalTable: "algorithms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlgorithmId = table.Column<long>(type: "bigint", nullable: false),
                    AlgorithmId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    StartN = table.Column<int>(type: "integer", nullable: false),
                    EndN = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ForceRecalculate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkSessions_algorithms_AlgorithmId1",
                        column: x => x.AlgorithmId1,
                        principalTable: "algorithms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId1 = table.Column<long>(type: "bigint", nullable: false),
                    BenchmarkRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    N = table.Column<int>(type: "integer", nullable: false),
                    ExecutionTimeMs = table.Column<double>(type: "double precision", nullable: false),
                    FromCache = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionRuns_BenchmarkSessions_SessionId1",
                        column: x => x.SessionId1,
                        principalTable: "BenchmarkSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRuns_benchmark_runs_BenchmarkRunId",
                        column: x => x.BenchmarkRunId,
                        principalTable: "benchmark_runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_algorithms_Code",
                table: "algorithms",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_benchmark_runs_AlgorithmId_N",
                table: "benchmark_runs",
                columns: new[] { "AlgorithmId", "N" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkSessions_AlgorithmId1",
                table: "BenchmarkSessions",
                column: "AlgorithmId1");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRuns_BenchmarkRunId",
                table: "SessionRuns",
                column: "BenchmarkRunId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRuns_SessionId1",
                table: "SessionRuns",
                column: "SessionId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionRuns");

            migrationBuilder.DropTable(
                name: "BenchmarkSessions");

            migrationBuilder.DropTable(
                name: "benchmark_runs");

            migrationBuilder.DropTable(
                name: "algorithms");
        }
    }
}
