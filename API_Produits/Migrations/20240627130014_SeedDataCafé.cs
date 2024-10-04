using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Produits.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataCafé : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CategoryType",
                value: "Moulu");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryType",
                value: "Grain");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CategoryType",
                value: "Décafeiné");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998), new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998), new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998), new DateTime(2024, 6, 27, 13, 0, 14, 689, DateTimeKind.Utc).AddTicks(1998) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CategoryType",
                value: "Boisson");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryType",
                value: "Boisson");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CategoryType",
                value: "Boisson");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519), new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519), new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519), new DateTime(2024, 6, 27, 12, 59, 10, 373, DateTimeKind.Utc).AddTicks(2519) });
        }
    }
}
