using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCallSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MainIntent",
                table: "InteractionAnalyses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CallSummaries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CallId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationSeconds = table.Column<double>(type: "float", nullable: false),
                    MessagesCount = table.Column<int>(type: "int", nullable: false),
                    FullTranscript = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessagesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFilesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioInfoJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummaryAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentimentScore = table.Column<double>(type: "float", nullable: false),
                    SentimentLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainTopicsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntentsDetectedJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionsPerformedJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyMomentsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelsUsedJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationRequired = table.Column<bool>(type: "bit", nullable: false),
                    EscalationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallSummaries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallSummaries");

            migrationBuilder.AlterColumn<string>(
                name: "MainIntent",
                table: "InteractionAnalyses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
