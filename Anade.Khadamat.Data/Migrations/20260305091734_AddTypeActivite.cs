using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class AddTypeActivite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeActivite",
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
                    table.PrimaryKey("PK_TypeActivite", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activites_TypeActiviteId",
                table: "Activites",
                column: "TypeActiviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_TypeActivite_TypeActiviteId",
                table: "Activites",
                column: "TypeActiviteId",
                principalTable: "TypeActivite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_TypeActivite_TypeActiviteId",
                table: "Activites");

            migrationBuilder.DropTable(
                name: "TypeActivite");

            migrationBuilder.DropIndex(
                name: "IX_Activites_TypeActiviteId",
                table: "Activites");
        }
    }
}
