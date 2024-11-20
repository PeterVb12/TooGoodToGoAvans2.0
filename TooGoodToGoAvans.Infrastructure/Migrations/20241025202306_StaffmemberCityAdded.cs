using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TooGoodToGoAvans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StaffmemberCityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNumber",
                table: "StaffMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StaffmemberCity",
                table: "StaffMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffmemberCity",
                table: "StaffMembers");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNumber",
                table: "StaffMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
