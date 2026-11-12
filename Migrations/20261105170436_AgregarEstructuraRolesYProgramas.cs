using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gestionConsejoProgramaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEstructuraRolesYProgramas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Rol_id_rol",
                schema: "perfil",
                table: "UsuarioRol");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Usuario_id_usuario",
                schema: "perfil",
                table: "UsuarioRol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRol",
                schema: "perfil",
                table: "UsuarioRol");

            migrationBuilder.DropColumn(
                name: "IdRol",
                schema: "perfil",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "nombre_rol",
                schema: "perfil",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "Semestre",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "codigo_estudiantil",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "plan_estudio",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "porcentaje_creditos_aprobados",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "programa",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "situacion_academica",
                schema: "perfil",
                table: "Estudiante");

            migrationBuilder.RenameTable(
                name: "UsuarioRol",
                schema: "perfil",
                newName: "Usuario_rol",
                newSchema: "perfil");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioRol_id_rol",
                schema: "perfil",
                table: "Usuario_rol",
                newName: "IX_Usuario_rol_id_rol");

            migrationBuilder.AlterColumn<string>(
                name: "contrasena",
                schema: "perfil",
                table: "Usuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                schema: "perfil",
                table: "Rol",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rol_nombre",
                schema: "perfil",
                table: "Rol",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "id_usuario_rol",
                schema: "perfil",
                table: "Usuario_rol",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "id_programa",
                schema: "perfil",
                table: "Usuario_rol",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario_rol",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_usuario_rol");

            migrationBuilder.CreateTable(
                name: "Programa",
                schema: "perfil",
                columns: table => new
                {
                    id_programa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programa", x => x.id_programa);
                });

            migrationBuilder.CreateTable(
                name: "Estudiante_programa",
                schema: "perfil",
                columns: table => new
                {
                    id_estudiante_programa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estudiante = table.Column<int>(type: "int", nullable: false),
                    id_programa = table.Column<int>(type: "int", nullable: false),
                    semestre = table.Column<int>(type: "int", nullable: false),
                    codigo_estudiantil = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    situacion_academica = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    porcentaje_creditos_aprobados = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    plan_estudio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiante_programa", x => x.id_estudiante_programa);
                    table.ForeignKey(
                        name: "FK_Estudiante_programa_Estudiante_id_estudiante",
                        column: x => x.id_estudiante,
                        principalSchema: "perfil",
                        principalTable: "Estudiante",
                        principalColumn: "id_estudiante",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estudiante_programa_Programa_id_programa",
                        column: x => x.id_programa,
                        principalSchema: "perfil",
                        principalTable: "Programa",
                        principalColumn: "id_programa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "perfil",
                table: "Programa",
                columns: new[] { "id_programa", "descripcion", "nombre" },
                values: new object[] { 1, "Ingeniería", "Ingeniería de Sistemas" });

            migrationBuilder.InsertData(
                schema: "perfil",
                table: "Rol",
                columns: new[] { "id_rol", "descripcion", "rol_nombre" },
                values: new object[,]
                {
                    { 1, "Rol Estudiante", "Estudiante" },
                    { 2, "Rol Coordinador", "Coordinador" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_rol_id_programa",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_programa");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_rol_id_usuario",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_programa_id_estudiante",
                schema: "perfil",
                table: "Estudiante_programa",
                column: "id_estudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_programa_id_programa",
                schema: "perfil",
                table: "Estudiante_programa",
                column: "id_programa");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_rol_Programa_id_programa",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_programa",
                principalSchema: "perfil",
                principalTable: "Programa",
                principalColumn: "id_programa");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_rol_Rol_id_rol",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_rol",
                principalSchema: "perfil",
                principalTable: "Rol",
                principalColumn: "id_rol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_rol_Usuario_id_usuario",
                schema: "perfil",
                table: "Usuario_rol",
                column: "id_usuario",
                principalSchema: "perfil",
                principalTable: "Usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_rol_Programa_id_programa",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_rol_Rol_id_rol",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_rol_Usuario_id_usuario",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropTable(
                name: "Estudiante_programa",
                schema: "perfil");

            migrationBuilder.DropTable(
                name: "Programa",
                schema: "perfil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario_rol",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_rol_id_programa",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_rol_id_usuario",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DeleteData(
                schema: "perfil",
                table: "Rol",
                keyColumn: "id_rol",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "perfil",
                table: "Rol",
                keyColumn: "id_rol",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "rol_nombre",
                schema: "perfil",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "id_usuario_rol",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.DropColumn(
                name: "id_programa",
                schema: "perfil",
                table: "Usuario_rol");

            migrationBuilder.RenameTable(
                name: "Usuario_rol",
                schema: "perfil",
                newName: "UsuarioRol",
                newSchema: "perfil");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_rol_id_rol",
                schema: "perfil",
                table: "UsuarioRol",
                newName: "IX_UsuarioRol_id_rol");

            migrationBuilder.AlterColumn<string>(
                name: "contrasena",
                schema: "perfil",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                schema: "perfil",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                schema: "perfil",
                table: "Rol",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_rol",
                schema: "perfil",
                table: "Rol",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Semestre",
                schema: "perfil",
                table: "Estudiante",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "codigo_estudiantil",
                schema: "perfil",
                table: "Estudiante",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "plan_estudio",
                schema: "perfil",
                table: "Estudiante",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "porcentaje_creditos_aprobados",
                schema: "perfil",
                table: "Estudiante",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "programa",
                schema: "perfil",
                table: "Estudiante",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "situacion_academica",
                schema: "perfil",
                table: "Estudiante",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRol",
                schema: "perfil",
                table: "UsuarioRol",
                columns: new[] { "id_usuario", "id_rol" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Rol_id_rol",
                schema: "perfil",
                table: "UsuarioRol",
                column: "id_rol",
                principalSchema: "perfil",
                principalTable: "Rol",
                principalColumn: "id_rol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Usuario_id_usuario",
                schema: "perfil",
                table: "UsuarioRol",
                column: "id_usuario",
                principalSchema: "perfil",
                principalTable: "Usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
