using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Inrfastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateTimeFieldsofVillas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Villas",
                newName: "Update_Date");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Villas",
                newName: "Created_Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Update_Date",
                table: "Villas",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Created_Date",
                table: "Villas",
                newName: "CreatedDate");
        }
    }
}
