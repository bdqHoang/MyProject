using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConversationDisplayName_ConversationId",
                table: "ConversationDisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationDisplayName_ConversationId",
                table: "ConversationDisplayName",
                column: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConversationDisplayName_ConversationId",
                table: "ConversationDisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationDisplayName_ConversationId",
                table: "ConversationDisplayName",
                column: "ConversationId",
                unique: true);
        }
    }
}
