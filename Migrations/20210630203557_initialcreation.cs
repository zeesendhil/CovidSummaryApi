using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidSummaryApi.Migrations
{
    public partial class initialcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    @new = table.Column<string>(name: "new", type: "nvarchar(max)", nullable: true),
                    active = table.Column<int>(type: "int", nullable: false),
                    critical = table.Column<int>(type: "int", nullable: false),
                    recovered = table.Column<int>(type: "int", nullable: false),
                    _1M_pop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Deaths",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    @new = table.Column<string>(name: "new", type: "nvarchar(max)", nullable: true),
                    _1M_pop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deaths", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    day = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _1M_pop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Summary",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    get = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parametersID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    results = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summary", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Summary_Parameters_parametersID",
                        column: x => x.parametersID,
                        principalTable: "Parameters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "errors",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_errors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_errors_Summary_SummaryID",
                        column: x => x.SummaryID,
                        principalTable: "Summary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Response",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    continent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    population = table.Column<int>(type: "int", nullable: false),
                    casesID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deathsID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    testsID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SummaryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Response", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Response_Cases_casesID",
                        column: x => x.casesID,
                        principalTable: "Cases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Response_Deaths_deathsID",
                        column: x => x.deathsID,
                        principalTable: "Deaths",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Response_Summary_SummaryID",
                        column: x => x.SummaryID,
                        principalTable: "Summary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Response_Tests_testsID",
                        column: x => x.testsID,
                        principalTable: "Tests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_errors_SummaryID",
                table: "errors",
                column: "SummaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Response_casesID",
                table: "Response",
                column: "casesID");

            migrationBuilder.CreateIndex(
                name: "IX_Response_deathsID",
                table: "Response",
                column: "deathsID");

            migrationBuilder.CreateIndex(
                name: "IX_Response_SummaryID",
                table: "Response",
                column: "SummaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Response_testsID",
                table: "Response",
                column: "testsID");

            migrationBuilder.CreateIndex(
                name: "IX_Summary_parametersID",
                table: "Summary",
                column: "parametersID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "errors");

            migrationBuilder.DropTable(
                name: "Response");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Deaths");

            migrationBuilder.DropTable(
                name: "Summary");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
