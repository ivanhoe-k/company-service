using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExchangeMicCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Isin",
                table: "Companies",
                column: "Isin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Ticker",
                table: "Companies",
                column: "Ticker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
