using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class fixmodelactivite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "structureCode",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "structureDesignation",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "structureCode",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "structureDesignation",
                table: "Activites");
        }
    }
}
