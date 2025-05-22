using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionCollectes.Migrations
{
    /// <inheritdoc />
    public partial class suppressionCentre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CentreId",
                table: "Collectes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CentreId",
                table: "Collectes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
