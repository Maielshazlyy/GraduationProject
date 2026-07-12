using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuCategories_MenuCategoryId",
                table: "MenuItems");

            migrationBuilder.AddColumn<string>(
                name: "InteractionId",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedOrderId",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketType",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiMetadataJson",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "Intent",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CallSessionId",
                table: "Interactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteractionType",
                table: "Interactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedOrderId",
                table: "Interactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedTicketId",
                table: "Interactions",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_InteractionId",
                table: "Feedbacks",
                column: "InteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Interactions_InteractionId",
                table: "Feedbacks",
                column: "InteractionId",
                principalTable: "Interactions",
                principalColumn: "InteractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuCategories_MenuCategoryId",
                table: "MenuItems",
                column: "MenuCategoryId",
                principalTable: "MenuCategories",
                principalColumn: "MenuCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Interactions_InteractionId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuCategories_MenuCategoryId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_InteractionId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "RelatedOrderId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AiMetadataJson",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AudioPath",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Intent",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CallSessionId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "InteractionType",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "RelatedOrderId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "RelatedTicketId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "Feedbacks");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuCategories_MenuCategoryId",
                table: "MenuItems",
                column: "MenuCategoryId",
                principalTable: "MenuCategories",
                principalColumn: "MenuCategoryId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
