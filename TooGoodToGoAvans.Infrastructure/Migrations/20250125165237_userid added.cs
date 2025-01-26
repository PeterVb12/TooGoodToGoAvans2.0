﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TooGoodToGoAvans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class useridadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Students");
        }
    }
}
