using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class RentPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    AdditionalDailyPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentPlans", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RentPlans",
                columns: new[] { "Id", "AdditionalDailyPrice", "CreatedAt", "Days", "Fee", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("426bf15e-4cf2-4713-af96-f800c7215cfa"), 50m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), 15, 40m, 28m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) },
                    { new Guid("ce5e3d9c-b16f-4a2c-bd57-9f75a4a25650"), 50m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), 7, 20m, 30m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) },
                    { new Guid("d507b101-dcdb-4d41-b85f-0c4e80ad15c7"), 50m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), 30, 60m, 22m, new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentPlans");
        }
    }
}
