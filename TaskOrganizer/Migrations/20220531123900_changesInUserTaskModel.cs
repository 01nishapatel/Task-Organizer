using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskOrganizer.Migrations
{
    public partial class changesInUserTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "User",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "User",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "User",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "ReporterID",
                table: "Task",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "User",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "User",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Task",
                newName: "ReporterID");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
