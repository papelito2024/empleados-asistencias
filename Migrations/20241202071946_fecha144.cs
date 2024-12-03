using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test2.Migrations
{
    /// <inheritdoc />
    public partial class fecha144 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Posiciones_PosicionId",
                table: "Empleados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posiciones",
                table: "Posiciones");

            migrationBuilder.RenameTable(
                name: "Posiciones",
                newName: "Posicion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posicion",
                table: "Posicion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Posicion_PosicionId",
                table: "Empleados",
                column: "PosicionId",
                principalTable: "Posicion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Posicion_PosicionId",
                table: "Empleados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posicion",
                table: "Posicion");

            migrationBuilder.RenameTable(
                name: "Posicion",
                newName: "Posiciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posiciones",
                table: "Posiciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Posiciones_PosicionId",
                table: "Empleados",
                column: "PosicionId",
                principalTable: "Posiciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
