using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class Add_MoisCloture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoisClotures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    Mois = table.Column<int>(type: "int", nullable: false),
                    IsCloture = table.Column<bool>(type: "bit", nullable: false),
                    DateCloture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloturePar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoisClotures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoisClotures_Annee_Mois",
                table: "MoisClotures",
                columns: new[] { "Annee", "Mois" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoisClotures");
        }
    }
}
