using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Inrfastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedVillasSeedbyAddingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { null, null });
        }
    }
}
