using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MingleZone.Migrations
{
    /// <inheritdoc />
    public partial class dateAddedToRequestsAndMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "MembershipRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CommunityMemberships",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "MembershipRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CommunityMemberships");
        }
    }
}
