﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnicalServiceTask.Data;

#nullable disable

namespace TechnicalServiceTask.Migrations
{
    [DbContext(typeof(AppEntity))]
    [Migration("20240308062127_RemovedActivityTable")]
    partial class RemovedActivityTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechnicalServiceTask.Data.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Code");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.System", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentSystemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("Code");

                    b.HasIndex("ParentSystemId");

                    b.ToTable("Systems");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovePersonId")
                        .HasColumnType("int");

                    b.Property<string>("ApprovePersonNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConfirmPersonId")
                        .HasColumnType("int");

                    b.Property<string>("ConfirmPersonNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatePersonId")
                        .HasColumnType("int");

                    b.Property<string>("CreatePersonNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("VerifyPersonId")
                        .HasColumnType("int");

                    b.Property<string>("VerifyPersonNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TechnicalServices");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalServiceBlock", b =>
                {
                    b.Property<int>("TechnicalServiceId")
                        .HasColumnType("int");

                    b.Property<int>("BlockId")
                        .HasColumnType("int");

                    b.HasKey("TechnicalServiceId", "BlockId");

                    b.HasIndex("BlockId");

                    b.ToTable("TechnicalServiceBlocks");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalServiceSystem", b =>
                {
                    b.Property<int>("TechnicalServiceId")
                        .HasColumnType("int");

                    b.Property<int>("SystemId")
                        .HasColumnType("int");

                    b.HasKey("TechnicalServiceId", "SystemId");

                    b.HasIndex("SystemId");

                    b.ToTable("TechnicalServiceSystems");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.System", b =>
                {
                    b.HasOne("TechnicalServiceTask.Data.System", "ParentSystem")
                        .WithMany()
                        .HasForeignKey("ParentSystemId");

                    b.Navigation("ParentSystem");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalServiceBlock", b =>
                {
                    b.HasOne("TechnicalServiceTask.Data.Block", "Block")
                        .WithMany()
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnicalServiceTask.Data.TechnicalService", "TechnicalService")
                        .WithMany("TechnicalServiceBlocks")
                        .HasForeignKey("TechnicalServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");

                    b.Navigation("TechnicalService");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalServiceSystem", b =>
                {
                    b.HasOne("TechnicalServiceTask.Data.System", "System")
                        .WithMany()
                        .HasForeignKey("SystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnicalServiceTask.Data.TechnicalService", "TechnicalService")
                        .WithMany("TechnicalServiceSystems")
                        .HasForeignKey("TechnicalServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("System");

                    b.Navigation("TechnicalService");
                });

            modelBuilder.Entity("TechnicalServiceTask.Data.TechnicalService", b =>
                {
                    b.Navigation("TechnicalServiceBlocks");

                    b.Navigation("TechnicalServiceSystems");
                });
#pragma warning restore 612, 618
        }
    }
}
