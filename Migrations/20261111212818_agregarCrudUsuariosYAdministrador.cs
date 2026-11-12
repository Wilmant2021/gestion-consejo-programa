using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionConsejoProgramaWeb.Migrations
{
    /// <inheritdoc />
    public partial class agregarCrudUsuariosYAdministrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "estado",
                schema: "perfil",
                table: "Usuario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Activo");

            migrationBuilder.InsertData(
                schema: "perfil",
                table: "Rol",
                columns: new[] { "id_rol", "descripcion", "rol_nombre" },
                values: new object[] { 3, "Rol Administrador del Sistema", "Administrador" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_correo_institucional",
                schema: "perfil",
                table: "Usuario",
                column: "correo_institucional",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_identity_provider_id",
                schema: "perfil",
                table: "Usuario",
                column: "identity_provider_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_correo_institucional",
                schema: "perfil",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_identity_provider_id",
                schema: "perfil",
                table: "Usuario");

            migrationBuilder.DeleteData(
                schema: "perfil",
                table: "Rol",
                keyColumn: "id_rol",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "estado",
                schema: "perfil",
                table: "Usuario");
        }
    }
}
