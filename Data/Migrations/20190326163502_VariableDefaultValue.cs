using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentAutomation.Data.Migrations
{
    public partial class VariableDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "TemplateVariable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "TemplateVariable");
        }
    }
}
