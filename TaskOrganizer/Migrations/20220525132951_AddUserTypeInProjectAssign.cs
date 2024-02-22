using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskOrganizer.Migrations
{
    public partial class AddUserTypeInProjectAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "ProjectAssign",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "ProjectAssign");
        }
    }
}
