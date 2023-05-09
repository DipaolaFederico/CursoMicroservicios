using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Transferencias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transferencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CuilOriginante = table.Column<string>(type: "TEXT", nullable: true),
                    CuilDestinatario = table.Column<string>(type: "TEXT", nullable: true),
                    CbuOrigen = table.Column<string>(type: "TEXT", nullable: true),
                    CbuDestino = table.Column<string>(type: "TEXT", nullable: true),
                    Importe = table.Column<double>(type: "REAL", nullable: false),
                    Concepto = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transferencias", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transferencias");
        }
    }
}
