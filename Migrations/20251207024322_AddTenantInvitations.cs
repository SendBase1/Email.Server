using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InviteeEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvitedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    InvitationToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantInvitations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 43, 20, 912, DateTimeKind.Utc).AddTicks(8293));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 43, 20, 912, DateTimeKind.Utc).AddTicks(8291));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 43, 20, 912, DateTimeKind.Utc).AddTicks(5366));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 43, 20, 912, DateTimeKind.Utc).AddTicks(8286));

            migrationBuilder.CreateIndex(
                name: "IX_TenantInvitations_TenantEmailStatus",
                table: "TenantInvitations",
                columns: new[] { "TenantId", "InviteeEmail", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantInvitations_TenantStatus",
                table: "TenantInvitations",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "UQ_TenantInvitations_Token",
                table: "TenantInvitations",
                column: "InvitationToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantInvitations");

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 38, 26, 287, DateTimeKind.Utc).AddTicks(2103));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 38, 26, 287, DateTimeKind.Utc).AddTicks(2101));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 38, 26, 287, DateTimeKind.Utc).AddTicks(457));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 2, 38, 26, 287, DateTimeKind.Utc).AddTicks(2099));
        }
    }
}
