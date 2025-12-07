using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Server.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyContactSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ContactSubmissions");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "ContactSubmissions");

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 7, 0, 47, 429, DateTimeKind.Utc).AddTicks(188));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 7, 0, 47, 429, DateTimeKind.Utc).AddTicks(187));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 7, 0, 47, 428, DateTimeKind.Utc).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 7, 0, 47, 429, DateTimeKind.Utc).AddTicks(183));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ContactSubmissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "ContactSubmissions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 6, 15, 10, 658, DateTimeKind.Utc).AddTicks(3538));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 6, 15, 10, 658, DateTimeKind.Utc).AddTicks(3536));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 6, 15, 10, 657, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 6, 15, 10, 658, DateTimeKind.Utc).AddTicks(3529));
        }
    }
}
