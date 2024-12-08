using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAUI_Nikitina_KP.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Letters",
                newName: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Letters",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Letters",
                newName: "Text");
        }
    }
}
