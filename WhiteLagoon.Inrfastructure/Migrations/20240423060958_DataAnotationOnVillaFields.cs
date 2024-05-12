using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataAnotationOnVillaFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4890), new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4902) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4905), new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4905) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4907), new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4907) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(843), new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(853) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(856), new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(856) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(858), new DateTime(2024, 4, 21, 10, 25, 34, 197, DateTimeKind.Local).AddTicks(858) });
        }
    }
}
