﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace TaskOrganizer.Migrations
{
    [DbContext(typeof(TaskOrganizerDBContext))]
    [Migration("20220504130703_test")]
    partial class test
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Database.Attachment", b =>
                {
                    b.Property<int>("AttachmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttachmentID"), 1L, 1);

                    b.Property<int>("AttachmentType")
                        .HasColumnType("int");

                    b.Property<string>("Attachments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttachmentID");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("Database.Comment", b =>
                {
                    b.Property<int>("CommentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentID"), 1L, 1);

                    b.Property<int>("AttachmentID")
                        .HasColumnType("int");

                    b.Property<int>("TaskID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentID");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Database.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectID"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ProjectID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Database.ProjectAssign", b =>
                {
                    b.Property<int>("ProjectAssignID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectAssignID"), 1L, 1);

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ProjectAssignID");

                    b.ToTable("ProjectAssign");
                });

            modelBuilder.Entity("Database.Task", b =>
                {
                    b.Property<int>("TaskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskID"), 1L, 1);

                    b.Property<int>("AssigneeID")
                        .HasColumnType("int");

                    b.Property<int>("AttachmentID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("ReporterID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TaskTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskType")
                        .HasColumnType("int");

                    b.HasKey("TaskID");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("Database.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ContactNo")
                        .HasColumnType("int");

                    b.Property<string>("EmailID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
