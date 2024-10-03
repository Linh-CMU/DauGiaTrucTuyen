using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class uDepsit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "402b0afa-53e0-400c-a6d8-3975439c54ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73bc3c3a-44bb-4bba-a3f3-e9dc4129532b");

            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "ListAuction",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    DID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RAID = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.DID);
                    table.ForeignKey(
                        name: "FK_Deposits_RegistAuction_RAID",
                        column: x => x.RAID,
                        principalTable: "RegistAuction",
                        principalColumn: "RAID");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "437239f0-3bc4-43d1-8e2b-145de5962614", null, "admin", "ADMIN" },
                    { "c170d16a-3c47-45a7-b022-647eef406241", null, "user", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_RAID",
                table: "Deposits",
                column: "RAID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "437239f0-3bc4-43d1-8e2b-145de5962614");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c170d16a-3c47-45a7-b022-647eef406241");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "ListAuction");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "402b0afa-53e0-400c-a6d8-3975439c54ca", null, "user", "USER" },
                    { "73bc3c3a-44bb-4bba-a3f3-e9dc4129532b", null, "admin", "ADMIN" }
                });
        }
    }
}
