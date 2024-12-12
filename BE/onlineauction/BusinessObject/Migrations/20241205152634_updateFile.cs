using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class updateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63b0d059-fe5d-4cd2-89af-1297acc5968a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e63922c8-9c9d-4103-9043-842a88b6e846");

            migrationBuilder.AddColumn<string>(
                name: "evidenceFile",
                table: "AuctionDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a0e41f9-5f7f-458e-afd5-e5883e764f5c", null, "admin", "ADMIN" },
                    { "f623d82c-fe34-4510-bb53-0f58c89fc0a8", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a0e41f9-5f7f-458e-afd5-e5883e764f5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f623d82c-fe34-4510-bb53-0f58c89fc0a8");

            migrationBuilder.DropColumn(
                name: "evidenceFile",
                table: "AuctionDetail");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63b0d059-fe5d-4cd2-89af-1297acc5968a", null, "admin", "ADMIN" },
                    { "e63922c8-9c9d-4103-9043-842a88b6e846", null, "user", "USER" }
                });
        }
    }
}
