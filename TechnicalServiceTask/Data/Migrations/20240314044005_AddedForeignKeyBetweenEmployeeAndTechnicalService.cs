using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalServiceTask.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyBetweenEmployeeAndTechnicalService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServices_ApprovePersonId",
                table: "TechnicalServices",
                column: "ApprovePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServices_ConfirmPersonId",
                table: "TechnicalServices",
                column: "ConfirmPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServices_CreatePersonId",
                table: "TechnicalServices",
                column: "CreatePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalServices_VerifyPersonId",
                table: "TechnicalServices",
                column: "VerifyPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalServices_Employees_ApprovePersonId",
                table: "TechnicalServices",
                column: "ApprovePersonId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalServices_Employees_ConfirmPersonId",
                table: "TechnicalServices",
                column: "ConfirmPersonId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalServices_Employees_CreatePersonId",
                table: "TechnicalServices",
                column: "CreatePersonId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalServices_Employees_VerifyPersonId",
                table: "TechnicalServices",
                column: "VerifyPersonId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalServices_Employees_ApprovePersonId",
                table: "TechnicalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalServices_Employees_ConfirmPersonId",
                table: "TechnicalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalServices_Employees_CreatePersonId",
                table: "TechnicalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalServices_Employees_VerifyPersonId",
                table: "TechnicalServices");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalServices_ApprovePersonId",
                table: "TechnicalServices");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalServices_ConfirmPersonId",
                table: "TechnicalServices");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalServices_CreatePersonId",
                table: "TechnicalServices");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalServices_VerifyPersonId",
                table: "TechnicalServices");
        }
    }
}
