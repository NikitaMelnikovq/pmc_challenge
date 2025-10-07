using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SteelShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nomenclature",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idcat = table.Column<int>(type: "integer", nullable: false),
                    idtype = table.Column<int>(type: "integer", nullable: false),
                    idtypenew = table.Column<int>(type: "integer", nullable: true),
                    productiontype = table.Column<string>(type: "text", nullable: true),
                    idfunctiontype = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    gost = table.Column<string>(type: "text", nullable: true),
                    formoflength = table.Column<string>(type: "text", nullable: true),
                    manufacturer = table.Column<string>(type: "text", nullable: true),
                    steelgrade = table.Column<string>(type: "text", nullable: true),
                    diameter = table.Column<double>(type: "double precision", nullable: true),
                    profilesize2 = table.Column<string>(type: "text", nullable: true),
                    pipewallthickness = table.Column<double>(type: "double precision", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    koef = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nomenclature", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    idstock = table.Column<int>(type: "integer", nullable: false),
                    pricet = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    pricelimitt1 = table.Column<double>(type: "double precision", nullable: true),
                    pricet1 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    pricelimitt2 = table.Column<double>(type: "double precision", nullable: true),
                    pricet2 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    pricem = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    pricelimitm1 = table.Column<double>(type: "double precision", nullable: true),
                    pricem1 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    pricelimitm2 = table.Column<double>(type: "double precision", nullable: true),
                    pricem2 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    nds = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => new { x.id, x.idstock });
                });

            migrationBuilder.CreateTable(
                name: "remnants",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    idstock = table.Column<int>(type: "integer", nullable: false),
                    instockt = table.Column<double>(type: "double precision", nullable: true),
                    instockm = table.Column<double>(type: "double precision", nullable: true),
                    soonarrivet = table.Column<double>(type: "double precision", nullable: true),
                    soonarrivem = table.Column<double>(type: "double precision", nullable: true),
                    reservedt = table.Column<double>(type: "double precision", nullable: true),
                    reservedm = table.Column<double>(type: "double precision", nullable: true),
                    undertheorder = table.Column<bool>(type: "boolean", nullable: true),
                    avgtubelength = table.Column<double>(type: "double precision", nullable: true),
                    avgtubeweight = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_remnants", x => new { x.id, x.idstock });
                });

            migrationBuilder.CreateTable(
                name: "stock",
                columns: table => new
                {
                    idstock = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stock = table.Column<string>(type: "text", nullable: false),
                    stockname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock", x => x.idstock);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    idtype = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    idparenttype = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_types", x => x.idtype);
                });

            migrationBuilder.CreateIndex(
                name: "IX_nomenclature_idtype_diameter_pipewallthickness_gost_steelgr~",
                table: "nomenclature",
                columns: new[] { "idtype", "diameter", "pipewallthickness", "gost", "steelgrade" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nomenclature");

            migrationBuilder.DropTable(
                name: "prices");

            migrationBuilder.DropTable(
                name: "remnants");

            migrationBuilder.DropTable(
                name: "stock");

            migrationBuilder.DropTable(
                name: "types");
        }
    }
}
