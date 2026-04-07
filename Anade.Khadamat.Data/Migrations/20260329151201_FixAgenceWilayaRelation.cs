using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class FixAgenceWilayaRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgenceWilayaId",
                table: "Activites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AgenceWilayas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenceWilayas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activites_AgenceWilayaId",
                table: "Activites",
                column: "AgenceWilayaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_AgenceWilayas_AgenceWilayaId",
                table: "Activites",
                column: "AgenceWilayaId",
                principalTable: "AgenceWilayas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_AgenceWilayas_AgenceWilayaId",
                table: "Activites");

            migrationBuilder.DropTable(
                name: "AgenceWilayas");

            migrationBuilder.DropIndex(
                name: "IX_Activites_AgenceWilayaId",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "AgenceWilayaId",
                table: "Activites");
        }
    }
}
