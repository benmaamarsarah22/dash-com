using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class supprimerSujetAccorddeReunionExterne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SujetAccord",
                table: "ActiviteReunionExternes");

            migrationBuilder.AlterColumn<string>(
                name: "structureDesignation",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "structureDesignation",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SujetAccord",
                table: "ActiviteReunionExternes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
