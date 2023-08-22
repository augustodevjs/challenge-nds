using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lista de Tarefas_Usuários_UserId",
                table: "Lista de Tarefas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Lista de Tarefas_AssignmentListId",
                table: "Tarefas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Usuários_UserId",
                table: "Tarefas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuários",
                table: "Usuários");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lista de Tarefas",
                table: "Lista de Tarefas");

            migrationBuilder.RenameTable(
                name: "Usuários",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Tarefas",
                newName: "Assignments");

            migrationBuilder.RenameTable(
                name: "Lista de Tarefas",
                newName: "AssignmentLists");

            migrationBuilder.RenameIndex(
                name: "IX_Tarefas_UserId",
                table: "Assignments",
                newName: "IX_Assignments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tarefas_AssignmentListId",
                table: "Assignments",
                newName: "IX_Assignments_AssignmentListId");

            migrationBuilder.RenameIndex(
                name: "IX_Lista de Tarefas_UserId",
                table: "AssignmentLists",
                newName: "IX_AssignmentLists_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentLists",
                table: "AssignmentLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentLists_Users_UserId",
                table: "AssignmentLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments",
                column: "AssignmentListId",
                principalTable: "AssignmentLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentLists_Users_UserId",
                table: "AssignmentLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentLists_AssignmentListId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentLists",
                table: "AssignmentLists");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Usuários");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "Tarefas");

            migrationBuilder.RenameTable(
                name: "AssignmentLists",
                newName: "Lista de Tarefas");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_UserId",
                table: "Tarefas",
                newName: "IX_Tarefas_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_AssignmentListId",
                table: "Tarefas",
                newName: "IX_Tarefas_AssignmentListId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentLists_UserId",
                table: "Lista de Tarefas",
                newName: "IX_Lista de Tarefas_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuários",
                table: "Usuários",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lista de Tarefas",
                table: "Lista de Tarefas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lista de Tarefas_Usuários_UserId",
                table: "Lista de Tarefas",
                column: "UserId",
                principalTable: "Usuários",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Lista de Tarefas_AssignmentListId",
                table: "Tarefas",
                column: "AssignmentListId",
                principalTable: "Lista de Tarefas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Usuários_UserId",
                table: "Tarefas",
                column: "UserId",
                principalTable: "Usuários",
                principalColumn: "Id");
        }
    }
}
