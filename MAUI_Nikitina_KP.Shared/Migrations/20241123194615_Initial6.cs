using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAUI_Nikitina_KP.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Initial6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Letters",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "To",
                table: "Letters");
        }
    }
}
