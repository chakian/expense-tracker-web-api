using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.Persistence.Migrations
{
    public partial class TokenString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenString",
                table: "UserInternalTokens",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenString",
                table: "UserInternalTokens");
        }
    }
}
