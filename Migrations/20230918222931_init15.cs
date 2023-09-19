using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UFOPay.Migrations
{
    /// <inheritdoc />
    public partial class init15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "apiKey",
                table: "BusinessBills",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "apiKey",
                table: "B2BKassa",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apiKey",
                table: "BusinessBills");

            migrationBuilder.DropColumn(
                name: "apiKey",
                table: "B2BKassa");
        }
    }
}
