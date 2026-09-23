using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlgoLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add2DAndOutliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutlier",
                table: "SessionRuns",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "M",
                table: "SessionRuns",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproximationModel",
                table: "BenchmarkSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "M",
                table: "benchmark_runs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InputArity",
                table: "algorithms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Константная функция" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Сумма элементов" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Произведение элементов" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Наивный полином" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Схема Горнера" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111116"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Сортировка пузырьком" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111117"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Быстрая сортировка" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111118"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Timsort" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111119"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 2, "Умножение матриц" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111a"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Плавная сортировка" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111b"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Решето Эратосфена" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111c"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Простое возведение в степень" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111d"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Рекурсивное возведение в степень" });

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111e"),
                columns: new[] { "InputArity", "Name" },
                values: new object[] { 1, "Быстрое рекурсивное возведение в степень" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutlier",
                table: "SessionRuns");

            migrationBuilder.DropColumn(
                name: "M",
                table: "SessionRuns");

            migrationBuilder.DropColumn(
                name: "ApproximationModel",
                table: "BenchmarkSessions");

            migrationBuilder.DropColumn(
                name: "M",
                table: "benchmark_runs");

            migrationBuilder.DropColumn(
                name: "InputArity",
                table: "algorithms");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Name",
                value: "Const Function");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "Name",
                value: "Sum Function");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "Name",
                value: "Product Function");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "Name",
                value: "Naive Polynomial");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
                column: "Name",
                value: "Horner Polynomial");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111116"),
                column: "Name",
                value: "Bubble Sort");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111117"),
                column: "Name",
                value: "Quick Sort");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111118"),
                column: "Name",
                value: "Tim Sort");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111119"),
                column: "Name",
                value: "Matrix Multiplication");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111a"),
                column: "Name",
                value: "Smooth Sort");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111b"),
                column: "Name",
                value: "Sieve of Eratosthenes");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111c"),
                column: "Name",
                value: "Simple Pow");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111d"),
                column: "Name",
                value: "Recursive Pow");

            migrationBuilder.UpdateData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111e"),
                column: "Name",
                value: "Quick Recursive Pow");
        }
    }
}
