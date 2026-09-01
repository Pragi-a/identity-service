using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Columnsync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "failed_at",
                schema: "auth",
                table: "outbox_messages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "next_attempt_at",
                schema: "auth",
                table: "outbox_messages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "processing_by",
                schema: "auth",
                table: "outbox_messages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "processing_started_at",
                schema: "auth",
                table: "outbox_messages",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "failed_at",
                schema: "auth",
                table: "outbox_messages");

            migrationBuilder.DropColumn(
                name: "next_attempt_at",
                schema: "auth",
                table: "outbox_messages");

            migrationBuilder.DropColumn(
                name: "processing_by",
                schema: "auth",
                table: "outbox_messages");

            migrationBuilder.DropColumn(
                name: "processing_started_at",
                schema: "auth",
                table: "outbox_messages");
        }
    }
}
