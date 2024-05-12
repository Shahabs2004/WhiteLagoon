using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2367), new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2377) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2379), new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2379) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2381), new DateTime(2024, 5, 6, 11, 59, 5, 101, DateTimeKind.Local).AddTicks(2381) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1331), new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1343) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1345), new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1345) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1347), new DateTime(2024, 5, 6, 11, 47, 7, 987, DateTimeKind.Local).AddTicks(1347) });
        }
    }
}
