using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskOrganizer.Migrations
{
    public partial class PopulateProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Project] (ProjectTitle,[Key],Duration,UserID,Description,StartDate,EndDate) VALUES('wwwokko','w1',3,1,'NOOOO','2022-02-08 09:03:01','2022-04-08 09:03:45')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
