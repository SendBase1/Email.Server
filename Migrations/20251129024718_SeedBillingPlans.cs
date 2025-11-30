using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Email.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedBillingPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BillingPlans",
                columns: new[] { "Id", "AllowsOverage", "AnalyticsRetentionDays", "CreatedAtUtc", "Description", "DisplayName", "HasDedicatedIp", "IncludedEmails", "IsActive", "IsTrial", "MaxApiKeys", "MaxDomains", "MaxTeamMembers", "MaxTemplates", "MaxWebhooks", "MonthlyPriceCents", "Name", "OverageRateCentsPer1K", "SortOrder", "StripeMeteredPriceId", "StripePriceId", "StripeProductId", "SupportLevel", "TrialDays", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), true, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Perfect for small projects and startups", "Starter", false, 25000, true, false, 2, 1, 1, 10, 2, 900, "starter", 40, 1, null, "price_starter", "prod_starter", "Email", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), true, 30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "For growing businesses with higher volume", "Growth", false, 100000, true, false, 10, 5, 5, 50, 5, 2900, "growth", 30, 2, null, "price_growth", "prod_growth", "Priority", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000003"), true, 90, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "For large organizations with custom needs", "Enterprise", true, 500000, true, false, 50, 999, 20, 200, 20, 9900, "enterprise", 20, 3, null, "price_enterprise", "prod_enterprise", "24/7", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 47, 15, 945, DateTimeKind.Utc).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 47, 15, 945, DateTimeKind.Utc).AddTicks(8036));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 47, 15, 945, DateTimeKind.Utc).AddTicks(1256));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 47, 15, 945, DateTimeKind.Utc).AddTicks(8029));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BillingPlans",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "BillingPlans",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "BillingPlans",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 34, 24, 599, DateTimeKind.Utc).AddTicks(4710));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 34, 24, 599, DateTimeKind.Utc).AddTicks(4708));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 34, 24, 598, DateTimeKind.Utc).AddTicks(4795));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 11, 29, 2, 34, 24, 599, DateTimeKind.Utc).AddTicks(4693));
        }
    }
}
