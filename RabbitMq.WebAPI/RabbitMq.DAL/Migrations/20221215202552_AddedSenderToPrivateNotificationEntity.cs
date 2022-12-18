using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabbitMq.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSenderToPrivateNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderId",
                table: "Notification",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_SenderId",
                table: "Notification",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_SenderId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_SenderId",
                table: "Notification");
        }
    }
}
