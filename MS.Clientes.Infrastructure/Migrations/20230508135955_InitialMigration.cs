using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Clientes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    Apellido = table.Column<string>(type: "TEXT", nullable: true),
                    Cuil = table.Column<string>(type: "TEXT", nullable: true),
                    TipoDocumento = table.Column<string>(type: "TEXT", nullable: true),
                    NroDocumento = table.Column<int>(type: "INTEGER", nullable: false),
                    EsEmpleadoBNA = table.Column<bool>(type: "INTEGER", nullable: false),
                    PaisOrigen = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
