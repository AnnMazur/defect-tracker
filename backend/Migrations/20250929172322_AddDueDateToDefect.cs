using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace defectTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddDueDateToDefect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Defects",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Defects");
        }
    }
}
