using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class updateCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49ea10aa-52ee-4ea7-a455-9422e6215a30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b23e588a-9759-4f0d-bb91-6754348f6efc");

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "UserOtp",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "UserOtp");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49ea10aa-52ee-4ea7-a455-9422e6215a30", null, "admin", "ADMIN" },
                    { "b23e588a-9759-4f0d-bb91-6754348f6efc", null, "user", "USER" }
                });
        }
    }
}
