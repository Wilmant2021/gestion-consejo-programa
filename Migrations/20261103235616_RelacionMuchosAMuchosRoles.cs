using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionConsejoProgramaWeb.Migrations
{
    /// <inheritdoc />
    public partial class RelacionMuchosAMuchosRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                schema: "perfil",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "perfil",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                schema: "perfil",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.id_usuario, x.id_rol });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_id_rol",
                        column: x => x.id_rol,
                        principalSchema: "perfil",
                        principalTable: "Rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "perfil",
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_id_rol",
                schema: "perfil",
                table: "UsuarioRol",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioRol",
                schema: "perfil");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "perfil");

            migrationBuilder.DropColumn(
                name: "IdRol",
                schema: "perfil",
                table: "Usuario");
        }
    }
}
