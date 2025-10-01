using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tria_2025.Migrations
{
    /// <inheritdoc />
    public partial class arrumandoFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Funcionario",
                table: "Funcionario");

            migrationBuilder.RenameTable(
                name: "Funcionario",
                newName: "FUNCIONARIO");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "FUNCIONARIO",
                newName: "SENHA");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "FUNCIONARIO",
                newName: "NOME");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "FUNCIONARIO",
                newName: "EMAIL");

            migrationBuilder.RenameColumn(
                name: "Cargo",
                table: "FUNCIONARIO",
                newName: "CARGO");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FUNCIONARIO",
                newName: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FUNCIONARIO",
                table: "FUNCIONARIO",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FUNCIONARIO",
                table: "FUNCIONARIO");

            migrationBuilder.RenameTable(
                name: "FUNCIONARIO",
                newName: "Funcionario");

            migrationBuilder.RenameColumn(
                name: "SENHA",
                table: "Funcionario",
                newName: "Senha");

            migrationBuilder.RenameColumn(
                name: "NOME",
                table: "Funcionario",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "Funcionario",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "CARGO",
                table: "Funcionario",
                newName: "Cargo");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Funcionario",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Funcionario",
                table: "Funcionario",
                column: "Id");
        }
    }
}
