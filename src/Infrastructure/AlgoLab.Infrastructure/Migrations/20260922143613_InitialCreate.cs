using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
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
                    StepsCount = table.Column<long>(type: "bigint", nullable: true),
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlgorithmId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartN = table.Column<int>(type: "integer", nullable: false),
                    EndN = table.Column<int>(type: "integer", nullable: false),
                    StartM = table.Column<int>(type: "integer", nullable: true),
                    EndM = table.Column<int>(type: "integer", nullable: true),
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
                        name: "FK_BenchmarkSessions_algorithms_AlgorithmId",
                        column: x => x.AlgorithmId,
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
                    BenchmarkRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    N = table.Column<int>(type: "integer", nullable: false),
                    ExecutionTimeMs = table.Column<double>(type: "double precision", nullable: true),
                    StepsCount = table.Column<long>(type: "bigint", nullable: true),
                    FromCache = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionRuns_BenchmarkSessions_SessionId",
                        column: x => x.SessionId,
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

            migrationBuilder.InsertData(
                table: "algorithms",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "const-function", "Const Function" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "sum-function", "Sum Function" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "product-function", "Product Function" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "naive-polynomial", "Naive Polynomial" },
                    { new Guid("11111111-1111-1111-1111-111111111115"), "horner-polynomial", "Horner Polynomial" },
                    { new Guid("11111111-1111-1111-1111-111111111116"), "bubble-sort", "Bubble Sort" },
                    { new Guid("11111111-1111-1111-1111-111111111117"), "quick-sort", "Quick Sort" },
                    { new Guid("11111111-1111-1111-1111-111111111118"), "tim-sort", "Tim Sort" },
                    { new Guid("11111111-1111-1111-1111-111111111119"), "multiply-matrix", "Matrix Multiplication" },
                    { new Guid("11111111-1111-1111-1111-11111111111a"), "smooth-sort", "Smooth Sort" },
                    { new Guid("11111111-1111-1111-1111-11111111111b"), "sieve-of-eratosthenes", "Sieve of Eratosthenes" },
                    { new Guid("11111111-1111-1111-1111-11111111111c"), "simple-pow", "Simple Pow" },
                    { new Guid("11111111-1111-1111-1111-11111111111d"), "recursive-pow", "Recursive Pow" },
                    { new Guid("11111111-1111-1111-1111-11111111111e"), "quick-recursive-pow", "Quick Recursive Pow" }
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
                name: "IX_BenchmarkSessions_AlgorithmId",
                table: "BenchmarkSessions",
                column: "AlgorithmId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRuns_BenchmarkRunId",
                table: "SessionRuns",
                column: "BenchmarkRunId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRuns_SessionId",
                table: "SessionRuns",
                column: "SessionId");
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
