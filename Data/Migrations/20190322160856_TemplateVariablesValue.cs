using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentAutomation.Data.Migrations
{
    public partial class TemplateVariablesValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "TemplateVariable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "TemplateVariable");
        }
    }
}
