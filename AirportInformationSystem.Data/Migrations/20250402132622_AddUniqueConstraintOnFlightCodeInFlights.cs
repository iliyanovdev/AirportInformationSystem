using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportInformationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintOnFlightCodeInFlights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightCode",
                table: "Flights",
                column: "FlightCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Flights_FlightCode",
                table: "Flights");
        }
    }
}
