using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlgoLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAlgorithm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "algorithms",
                columns: new[] { "Id", "Code", "InputArity", "Name" },
                values: new object[] { new Guid("11111111-1111-1111-1111-11111111111f"), "atkin-sieve", 1, "Решето Аткина" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "algorithms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-11111111111f"));
        }
    }
}
