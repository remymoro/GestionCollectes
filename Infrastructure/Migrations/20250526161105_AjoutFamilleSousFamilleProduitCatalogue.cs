using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionCollectes.Migrations
{
    /// <inheritdoc />
    public partial class AjoutFamilleSousFamilleProduitCatalogue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SousFamilles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousFamilles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SousFamilles_Familles_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Familles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProduitsCatalogue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodeBarre = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilleId = table.Column<int>(type: "int", nullable: false),
                    SousFamilleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduitsCatalogue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProduitsCatalogue_Familles_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Familles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProduitsCatalogue_SousFamilles_SousFamilleId",
                        column: x => x.SousFamilleId,
                        principalTable: "SousFamilles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Familles_Nom",
                table: "Familles",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProduitsCatalogue_CodeBarre",
                table: "ProduitsCatalogue",
                column: "CodeBarre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProduitsCatalogue_FamilleId",
                table: "ProduitsCatalogue",
                column: "FamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitsCatalogue_SousFamilleId",
                table: "ProduitsCatalogue",
                column: "SousFamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_SousFamilles_FamilleId",
                table: "SousFamilles",
                column: "FamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_SousFamilles_Nom_FamilleId",
                table: "SousFamilles",
                columns: new[] { "Nom", "FamilleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProduitsCatalogue");

            migrationBuilder.DropTable(
                name: "SousFamilles");

            migrationBuilder.DropTable(
                name: "Familles");
        }
    }
}
