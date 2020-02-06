using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.Persistence.Migrations
{
    public partial class BudgetUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "BudgetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRead",
                table: "BudgetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanWrite",
                table: "BudgetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "BudgetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "BudgetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "BudgetUsers");

            migrationBuilder.DropColumn(
                name: "CanRead",
                table: "BudgetUsers");

            migrationBuilder.DropColumn(
                name: "CanWrite",
                table: "BudgetUsers");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "BudgetUsers");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "BudgetUsers");
        }
    }
}
