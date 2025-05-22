using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionCollectes.Migrations
{
    /// <inheritdoc />
    public partial class SuppressionLieuEtCentreDansCollecte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collectes_Centres_CentreId",
                table: "Collectes");

            migrationBuilder.DropIndex(
                name: "IX_Collectes_CentreId",
                table: "Collectes");

            migrationBuilder.DropColumn(
                name: "Lieu",
                table: "Collectes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lieu",
                table: "Collectes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Collectes_CentreId",
                table: "Collectes",
                column: "CentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collectes_Centres_CentreId",
                table: "Collectes",
                column: "CentreId",
                principalTable: "Centres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
