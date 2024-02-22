using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskOrganizer.Migrations
{
   
    using System;
    using System.Data.Entity.Migrations;
    public partial class PopulateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.Sql("INSERT INTO [User] (EmailID,Password,FirstName,LastName,BirthDate,ContactNo,Gender,Address,UserType) VALUES ('abc12@gmail.com','abc','Nidhi','Patel','2001-12-08 09:03:01',123456789,2,'Surat',2)");



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
