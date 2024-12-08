using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAUI_Nikitina_KP.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "Letters",
                newName: "ToWho");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Letters",
                newName: "FromWho");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToWho",
                table: "Letters",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "FromWho",
                table: "Letters",
                newName: "From");
        }
    }
}
