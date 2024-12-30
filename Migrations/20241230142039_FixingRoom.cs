using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixingRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Rooms",
                newName: "IsAvaiable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvaiable",
                table: "Rooms",
                newName: "IsAvailable");
        }
    }
}
