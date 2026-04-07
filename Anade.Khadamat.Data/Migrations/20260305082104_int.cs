using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class @int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StructureId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeActiviteId = table.Column<int>(type: "int", nullable: false),
                    DateActivite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sujet = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Lieu = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Organisateurs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Participants = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreVisiteurs = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteForums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteForums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteForums_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteJourneeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteJourneeInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteJourneeInfos_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivitePresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitePresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitePresses_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteRadios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false),
                    StationRadio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Intervenants = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteRadios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteRadios_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteReunionExternes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false),
                    SujetAccord = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteReunionExternes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteReunionExternes_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteSalons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteSalons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteSalons_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteTVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiviteId = table.Column<int>(type: "int", nullable: false),
                    ChaineTV = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiviteTVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiviteTVs_Activites_ActiviteId",
                        column: x => x.ActiviteId,
                        principalTable: "Activites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteForums_ActiviteId",
                table: "ActiviteForums",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteJourneeInfos_ActiviteId",
                table: "ActiviteJourneeInfos",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitePresses_ActiviteId",
                table: "ActivitePresses",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteRadios_ActiviteId",
                table: "ActiviteRadios",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteReunionExternes_ActiviteId",
                table: "ActiviteReunionExternes",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteSalons_ActiviteId",
                table: "ActiviteSalons",
                column: "ActiviteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteTVs_ActiviteId",
                table: "ActiviteTVs",
                column: "ActiviteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiviteForums");

            migrationBuilder.DropTable(
                name: "ActiviteJourneeInfos");

            migrationBuilder.DropTable(
                name: "ActivitePresses");

            migrationBuilder.DropTable(
                name: "ActiviteRadios");

            migrationBuilder.DropTable(
                name: "ActiviteReunionExternes");

            migrationBuilder.DropTable(
                name: "ActiviteSalons");

            migrationBuilder.DropTable(
                name: "ActiviteTVs");

            migrationBuilder.DropTable(
                name: "Activites");
        }
    }
}
