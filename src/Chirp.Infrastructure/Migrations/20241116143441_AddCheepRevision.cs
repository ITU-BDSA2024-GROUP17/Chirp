using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheepRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Cheeps");

            migrationBuilder.CreateTable(
                name: "CheepRevision",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheepId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheepRevision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheepRevision_Cheeps_CheepId",
                        column: x => x.CheepId,
                        principalTable: "Cheeps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheepRevision_CheepId",
                table: "CheepRevision",
                column: "CheepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheepRevision");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Cheeps",
                type: "TEXT",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "Cheeps",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
