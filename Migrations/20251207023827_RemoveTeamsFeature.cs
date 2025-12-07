using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTeamsFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKeys_Teams_TeamId",
                table: "ApiKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Teams_TeamId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Teams_TeamId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookEndpoints_Teams_TeamId",
                table: "WebhookEndpoints");

            migrationBuilder.DropTable(
                name: "TeamInvitations");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_WebhookEndpoints_TeamId",
                table: "WebhookEndpoints");

            migrationBuilder.DropIndex(
                name: "IX_Templates_TeamId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Domains_TeamId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_ApiKeys_TeamId",
                table: "ApiKeys");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "WebhookEndpoints");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Domains");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "ApiKeys");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "WebhookEndpoints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Templates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Domains",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "ApiKeys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcceptedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvitationToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InvitedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    InviteeEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamInvitations_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamInvitations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    InvitedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    JoinedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserDisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "ap-southeast-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 0, 31, 41, 493, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "eu-west-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 0, 31, 41, 493, DateTimeKind.Utc).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-east-1",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 0, 31, 41, 493, DateTimeKind.Utc).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "RegionsCatalog",
                keyColumn: "Region",
                keyValue: "us-west-2",
                column: "CreatedAtUtc",
                value: new DateTime(2025, 12, 7, 0, 31, 41, 493, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEndpoints_TeamId",
                table: "WebhookEndpoints",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TeamId",
                table: "Templates",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_TeamId",
                table: "Domains",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_TeamId",
                table: "ApiKeys",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvitations_TeamEmailStatus",
                table: "TeamInvitations",
                columns: new[] { "TeamId", "InviteeEmail", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvitations_TenantStatus",
                table: "TeamInvitations",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "UQ_TeamInvitations_Token",
                table: "TeamInvitations",
                column: "InvitationToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_Email",
                table: "TeamMembers",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_User",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TenantDefault",
                table: "Teams",
                columns: new[] { "TenantId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "UQ_Teams_TenantName",
                table: "Teams",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKeys_Teams_TeamId",
                table: "ApiKeys",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Teams_TeamId",
                table: "Domains",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Teams_TeamId",
                table: "Templates",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookEndpoints_Teams_TeamId",
                table: "WebhookEndpoints",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
