using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddInteractionAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InteractionAnalyses",
                columns: table => new
                {
                    InteractionAnalysisId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InteractionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusinessId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentimentScore = table.Column<double>(type: "float", nullable: false),
                    SentimentLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntentsDetectedJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainTopicsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyMomentsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionAnalyses", x => x.InteractionAnalysisId);
                    table.ForeignKey(
                        name: "FK_InteractionAnalyses_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InteractionAnalyses_Interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "Interactions",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InteractionAnalyses_BusinessId",
                table: "InteractionAnalyses",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionAnalyses_InteractionId",
                table: "InteractionAnalyses",
                column: "InteractionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InteractionAnalyses");
        }
    }
}
