using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.Persistence.Migrations
{
    public partial class UserInternalToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInternalTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Issuer = table.Column<string>(nullable: false),
                    CreatingIp = table.Column<string>(nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    LastUsedDate = table.Column<DateTime>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInternalTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInternalTokens");
        }
    }
}
