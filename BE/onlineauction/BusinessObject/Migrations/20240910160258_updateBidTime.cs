using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class updateBidTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17260635-5001-4dd8-9505-1a6f164f6ce9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8a36b8b-188e-4a01-8b73-f6e811b777e8");

            migrationBuilder.AlterColumn<string>(
                name: "BidTime",
                table: "Bet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "402b0afa-53e0-400c-a6d8-3975439c54ca", null, "user", "USER" },
                    { "73bc3c3a-44bb-4bba-a3f3-e9dc4129532b", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "402b0afa-53e0-400c-a6d8-3975439c54ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73bc3c3a-44bb-4bba-a3f3-e9dc4129532b");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BidTime",
                table: "Bet",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17260635-5001-4dd8-9505-1a6f164f6ce9", null, "admin", "ADMIN" },
                    { "f8a36b8b-188e-4a01-8b73-f6e811b777e8", null, "user", "USER" }
                });
        }
    }
}
