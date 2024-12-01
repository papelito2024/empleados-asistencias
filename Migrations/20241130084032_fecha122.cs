using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test2.Migrations
{
    /// <inheritdoc />
    public partial class fecha122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Departamentos_DepartamentoID",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Posiciones_PosicionID",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "PosicionID",
                table: "Empleados",
                newName: "PosicionId");

            migrationBuilder.RenameColumn(
                name: "DepartamentoID",
                table: "Empleados",
                newName: "DepartamentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_PosicionID",
                table: "Empleados",
                newName: "IX_Empleados_PosicionId");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_DepartamentoID",
                table: "Empleados",
                newName: "IX_Empleados_DepartamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Departamentos_DepartamentoId",
                table: "Empleados",
                column: "DepartamentoId",
                principalTable: "Departamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Posiciones_PosicionId",
                table: "Empleados",
                column: "PosicionId",
                principalTable: "Posiciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Departamentos_DepartamentoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Posiciones_PosicionId",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "PosicionId",
                table: "Empleados",
                newName: "PosicionID");

            migrationBuilder.RenameColumn(
                name: "DepartamentoId",
                table: "Empleados",
                newName: "DepartamentoID");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_PosicionId",
                table: "Empleados",
                newName: "IX_Empleados_PosicionID");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_DepartamentoId",
                table: "Empleados",
                newName: "IX_Empleados_DepartamentoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Departamentos_DepartamentoID",
                table: "Empleados",
                column: "DepartamentoID",
                principalTable: "Departamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Posiciones_PosicionID",
                table: "Empleados",
                column: "PosicionID",
                principalTable: "Posiciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
