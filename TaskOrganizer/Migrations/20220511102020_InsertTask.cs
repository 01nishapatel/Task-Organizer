using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskOrganizer.Migrations
{
    public partial class InsertTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Task VALUES (1,1,2,1,'Coding',1,'aaaaaa',2,0,'2022-05-11 09:03:01','2022-05-13 09:03:01')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
