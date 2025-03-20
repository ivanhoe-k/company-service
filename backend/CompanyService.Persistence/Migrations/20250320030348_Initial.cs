using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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

#pragma warning disable SA1118 // Parameter should not span multiple lines
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "ExchangeMicCode", "Isin", "Name", "Ticker", "Website" },
                values: new object[,]
                {
                    { new Guid("3e5b91e5-6a2f-4f77-8b25-4a6bca7263b1"), "XNAS", "US0378331005", "Apple Inc.", "AAPL", "https://www.apple.com/" },
                    { new Guid("3f1e3c45-44bb-40b9-be5e-0e54d36cfb49"), "XNAS", "US67066G1040", "NVIDIA Corporation", "NVDA", null },
                    { new Guid("56d9f87d-ec24-4f88-8339-7e2a9d9d50f2"), "XNYS", "US92826C8394", "Visa Inc.", "V", "https://www.visa.com/" },
                    { new Guid("66e0b1f9-13b3-4b63-b9e6-77daa21e2f7f"), "XNAS", "US5949181045", "Microsoft Corporation", "MSFT", "https://www.microsoft.com/" },
                    { new Guid("7cb6e2c8-8f9d-40e8-bbd6-54730b2c2c0a"), "XNYS", "US4781601046", "Johnson & Johnson", "JNJ", null },
                    { new Guid("847e915f-56a7-4c8d-b7d1-abf2e1d496ad"), "XNAS", "US02079K3059", "Alphabet Inc. (Google)", "GOOGL", "https://www.abc.xyz/" },
                    { new Guid("9fbaa3c6-ce8e-48b6-908e-6baf6a0a3db7"), "XNYS", "US1912161007", "The Coca-Cola Company", "KO", "https://www.coca-colacompany.com/" },
                    { new Guid("a2828bfc-254b-4df1-9a8a-95f8f0dccf47"), "XNAS", "US0231351067", "Amazon.com Inc.", "AMZN", "https://www.amazon.com/" },
                    { new Guid("a56f7c6c-88f8-4e77-96d8-58f82c4d3b94"), "XNAS", "US30303M1027", "Meta Platforms Inc. (Facebook)", "META", null },
                    { new Guid("d8a2fe5f-9d3c-4b17-ae9c-46c6b8b3b6a2"), "XNAS", "US88160R1014", "Tesla Inc.", "TSLA", "https://www.tesla.com/" }
                });
#pragma warning restore SA1118 // Parameter should not span multiple lines

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
