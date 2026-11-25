using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionConsejoProgramaWeb.Migrations.RequestsDb
{
    /// <inheritdoc />
    public partial class AddRequestsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "requests");

            migrationBuilder.CreateTable(
                name: "Tipo_solicitud",
                schema: "requests",
                columns: table => new
                {
                    id_tipo_solicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    drescripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_solicitud", x => x.id_tipo_solicitud);
                });

            migrationBuilder.CreateTable(
                name: "Sub_tipo_solicitud",
                schema: "requests",
                columns: table => new
                {
                    id_sub_tipo_solicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_tipo_soliciud = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sub_tipo_solicitud", x => x.id_sub_tipo_solicitud);
                    table.ForeignKey(
                        name: "FK_Sub_tipo_solicitud_Tipo_solicitud_id_tipo_soliciud",
                        column: x => x.id_tipo_soliciud,
                        principalSchema: "requests",
                        principalTable: "Tipo_solicitud",
                        principalColumn: "id_tipo_solicitud",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sub_tipo_solicitud_id_tipo_soliciud",
                schema: "requests",
                table: "Sub_tipo_solicitud",
                column: "id_tipo_soliciud");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sub_tipo_solicitud",
                schema: "requests");

            migrationBuilder.DropTable(
                name: "Tipo_solicitud",
                schema: "requests");
        }
    }
}
