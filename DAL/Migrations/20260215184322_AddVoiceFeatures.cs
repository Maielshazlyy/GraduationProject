using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVoiceFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add CallSessionId to Interactions table
            migrationBuilder.AddColumn<string>(
                name: "CallSessionId",
                table: "Interactions",
                type: "nvarchar(max)",
                nullable: true);

            // Add AudioPath and ConfidenceScore to Messages table
            migrationBuilder.AddColumn<string>(
                name: "AudioPath",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConfidenceScore",
                table: "Messages",
                type: "float",
                nullable: true);

            // Add InteractionId and SentimentScore to Feedbacks table
            migrationBuilder.AddColumn<string>(
                name: "InteractionId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SentimentScore",
                table: "Feedbacks",
                type: "float",
                nullable: true);

            // Create index for InteractionId in Feedbacks
            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_InteractionId",
                table: "Feedbacks",
                column: "InteractionId");

            // Add foreign key relationship between Feedbacks and Interactions
            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Interactions_InteractionId",
                table: "Feedbacks",
                column: "InteractionId",
                principalTable: "Interactions",
                principalColumn: "InteractionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Interactions_InteractionId",
                table: "Feedbacks");

            // Remove index
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_InteractionId",
                table: "Feedbacks");

            // Remove columns from Feedbacks
            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "Feedbacks");

            // Remove columns from Messages
            migrationBuilder.DropColumn(
                name: "AudioPath",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "Messages");

            // Remove column from Interactions
            migrationBuilder.DropColumn(
                name: "CallSessionId",
                table: "Interactions");
        }
    }
}

