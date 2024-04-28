using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Inrfastructure.Migrations
{
    /// <inheritdoc />
    public partial class addVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Villas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    Villa_Number = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.Villa_Number);
                    table.ForeignKey(
                        name: "FK_VillaNumbers_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "VillaNumbers",
                columns: new[] { "Villa_Number", "SpecialDetails", "VillaId" },
                values: new object[,]
                {
                    { 101, null, 1 },
                    { 102, null, 1 },
                    { 201, null, 2 },
                    { 202, null, 2 },
                    { 301, null, 3 },
                    { 302, null, 3 }
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1241), "Luxurious villa fit for royalty.", new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1256) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1258), "Exquisite villa with a private pool.", new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1258) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1259), "Opulent villa with a spacious pool.", new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1260) });

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillaId",
                table: "VillaNumbers",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4890), "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.", new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4902) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4905), "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.", new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4905) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Description", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4907), "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.", new DateTime(2024, 4, 23, 9, 39, 57, 393, DateTimeKind.Local).AddTicks(4907) });
        }
    }
}
