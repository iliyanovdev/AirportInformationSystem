using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportInformationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCancelledToFlightRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "FlightRoutes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "FlightRoutes");
        }
    }
}
