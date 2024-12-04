using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class updateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673257ad-0e95-4e9f-a3bd-8d0828381c8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9db9844-7962-429f-b266-ba83132e2f83");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BidTime",
                table: "PlacingABid",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63b0d059-fe5d-4cd2-89af-1297acc5968a", null, "admin", "ADMIN" },
                    { "e63922c8-9c9d-4103-9043-842a88b6e846", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63b0d059-fe5d-4cd2-89af-1297acc5968a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e63922c8-9c9d-4103-9043-842a88b6e846");

            migrationBuilder.AlterColumn<string>(
                name: "BidTime",
                table: "PlacingABid",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "673257ad-0e95-4e9f-a3bd-8d0828381c8c", null, "user", "USER" },
                    { "a9db9844-7962-429f-b266-ba83132e2f83", null, "admin", "ADMIN" }
                });
        }
    }
}
