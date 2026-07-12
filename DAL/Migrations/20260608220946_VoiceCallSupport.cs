using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class VoiceCallSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingUrl",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "CallSummaries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InteractionId",
                table: "CallSummaries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CallSummaries_BusinessId",
                table: "CallSummaries",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_CallSummaries_InteractionId",
                table: "CallSummaries",
                column: "InteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CallSummaries_Businesses_BusinessId",
                table: "CallSummaries",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CallSummaries_Interactions_InteractionId",
                table: "CallSummaries",
                column: "InteractionId",
                principalTable: "Interactions",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CallSummaries_Businesses_BusinessId",
                table: "CallSummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_CallSummaries_Interactions_InteractionId",
                table: "CallSummaries");

            migrationBuilder.DropIndex(
                name: "IX_CallSummaries_BusinessId",
                table: "CallSummaries");

            migrationBuilder.DropIndex(
                name: "IX_CallSummaries_InteractionId",
                table: "CallSummaries");

            migrationBuilder.DropColumn(
                name: "MeetingUrl",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "CallSummaries");

            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "CallSummaries");
        }
    }
}
