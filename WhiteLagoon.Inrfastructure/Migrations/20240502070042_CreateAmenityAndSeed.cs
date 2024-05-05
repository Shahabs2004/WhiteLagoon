using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Inrfastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAmenityAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "Name", "VillaId" },
                values: new object[,]
                {
                    { 1, null, "Private Pool", 1 },
                    { 2, null, "Ocean View", 1 },
                    { 3, null, "Garden", 1 },
                    { 4, null, "Jacuzzi", 1 },
                    { 5, null, "Private Pool", 2 },
                    { 6, null, "Ocean View", 2 },
                    { 7, null, "Garden", 2 },
                    { 8, null, "Jacuzzi", 2 },
                    { 9, null, "Private Pool", 3 },
                    { 10, null, "Ocean View", 3 },
                    { 11, null, "Garden", 3 },
                    { 12, null, "Jacuzzi", 3 }
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4220), new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4229) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4231), new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4232) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4233), new DateTime(2024, 5, 2, 10, 30, 40, 815, DateTimeKind.Local).AddTicks(4233) });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_VillaId",
                table: "Amenities",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1241), new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1256) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1258), new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1258) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created_Date", "Update_Date" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1259), new DateTime(2024, 4, 28, 11, 3, 44, 994, DateTimeKind.Local).AddTicks(1260) });
        }
    }
}
