﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalServiceTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicalServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                    table.UniqueConstraint("AK_Blocks_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PIN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ParentSystemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems", x => x.Id);
                    table.UniqueConstraint("AK_Systems_Code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Systems_Systems_ParentSystemId",
                        column: x => x.ParentSystemId,
                        principalTable: "Systems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TechnicalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatePersonId = table.Column<int>(type: "int", nullable: false),
                    CreatePersonNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPersonId = table.Column<int>(type: "int", nullable: false),
                    ConfirmPersonNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovePersonId = table.Column<int>(type: "int", nullable: false),
                    ApprovePersonNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifyPersonId = table.Column<int>(type: "int", nullable: false),
                    VerifyPersonNames = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalServiceBlocks",
                columns: table => new
                {
                    TechnicalServiceId = table.Column<int>(type: "int", nullable: false),
                    BlockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalServiceBlocks", x => new { x.TechnicalServiceId, x.BlockId });
                    table.ForeignKey(
                        name: "FK_TechnicalServiceBlocks_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalServiceBlocks_TechnicalServices_TechnicalServiceId",
                        column: x => x.TechnicalServiceId,
                        principalTable: "TechnicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalServiceSystems",
                columns: table => new
                {
                    TechnicalServiceId = table.Column<int>(type: "int", nullable: false),
                    SystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalServiceSystems", x => new { x.TechnicalServiceId, x.SystemId });
                    table.ForeignKey(
                        name: "FK_TechnicalServiceSystems_Systems_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Systems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalServiceSystems_TechnicalServices_TechnicalServiceId",
                        column: x => x.TechnicalServiceId,
                        principalTable: "TechnicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Systems_ParentSystemId",
                table: "Systems",
                column: "ParentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServiceBlocks_BlockId",
                table: "TechnicalServiceBlocks",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServiceSystems_SystemId",
                table: "TechnicalServiceSystems",
                column: "SystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TechnicalServiceBlocks");

            migrationBuilder.DropTable(
                name: "TechnicalServiceSystems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Systems");

            migrationBuilder.DropTable(
                name: "TechnicalServices");
        }
    }
}
