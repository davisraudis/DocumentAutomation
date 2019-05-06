using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentAutomation.Data.Migrations
{
    public partial class VariableDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TemplateVariable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TemplateVariable");
        }
    }
}
