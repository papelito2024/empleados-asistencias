using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace test2.Migrations
{
    /// <inheritdoc />
    public partial class addtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpleadoId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FechaControl = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistencias_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_EmpleadoId",
                table: "Asistencias",
                column: "EmpleadoId");

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
                name: "FK_Empleados_Posiciones_PosicionId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "Asistencias");

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
    }
}
