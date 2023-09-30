using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class addCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments",
                column: "AssignmentListId",
                principalTable: "AssignmentLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments",
                column: "AssignmentListId",
                principalTable: "AssignmentLists",
                principalColumn: "Id");
        }
    }
}
