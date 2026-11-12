using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionConsejoProgramaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIdentityProviderRemoverContrasena : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contrasena",
                schema: "perfil",
                table: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "identity_provider_id",
                schema: "perfil",
                table: "Usuario",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "identity_provider_id",
                schema: "perfil",
                table: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "contrasena",
                schema: "perfil",
                table: "Usuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
