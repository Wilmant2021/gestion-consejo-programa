using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionConsejoProgramaWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "perfil");

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "perfil",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    segundo_nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    segundo_apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    correo_institucional = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Estudiante",
                schema: "perfil",
                columns: table => new
                {
                    id_estudiante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    programa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    codigo_estudiantil = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    situacion_academica = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Semestre = table.Column<int>(type: "int", nullable: false),
                    porcentaje_creditos_aprobados = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    plan_estudio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiante", x => x.id_estudiante);
                    table.ForeignKey(
                        name: "FK_Estudiante_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "perfil",
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_id_usuario",
                schema: "perfil",
                table: "Estudiante",
                column: "id_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estudiante",
                schema: "perfil");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "perfil");
        }
    }
}
