using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TooGoodToGoAvans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Citypropertyaddedtopackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityLocation",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityLocation",
                table: "Packages");
        }
    }
}
