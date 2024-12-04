using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class updateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f2d897e-02bf-411c-900d-fc1508aeda71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3bfff3b-e2a8-4644-8dad-8666960d1399");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "RegistAuction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Notications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Feedback",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "AuctionDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "AccountDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "396029a9-bcbf-4b54-877b-b51de790a052", null, "user", "USER" },
                    { "98939845-60c2-410b-b7cc-dc53f85dec00", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "396029a9-bcbf-4b54-877b-b51de790a052");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98939845-60c2-410b-b7cc-dc53f85dec00");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "RegistAuction");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Notications");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AuctionDetail");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AccountDetails");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f2d897e-02bf-411c-900d-fc1508aeda71", null, "admin", "ADMIN" },
                    { "b3bfff3b-e2a8-4644-8dad-8666960d1399", null, "user", "USER" }
                });
        }
    }
}
