﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirportInformationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintOnEmailInApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");
        }
    }
}
