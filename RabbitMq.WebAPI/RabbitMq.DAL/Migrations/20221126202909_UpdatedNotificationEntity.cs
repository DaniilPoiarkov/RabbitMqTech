using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RabbitMq.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SimpleNotifications",
                table: "SimpleNotifications");

            migrationBuilder.RenameTable(
                name: "SimpleNotifications",
                newName: "Notification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecieverId",
                table: "Notification",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderId",
                table: "Notification",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_RecieverId",
                table: "Notification",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Notification_Users_RecieverId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_SenderId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_RecieverId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_SenderId",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "SimpleNotifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SimpleNotifications",
                table: "SimpleNotifications",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PublicNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotifications", x => x.Id);
                });
        }
    }
}
