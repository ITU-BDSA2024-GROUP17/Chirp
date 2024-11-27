using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheepComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheepId",
                table: "Cheeps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_CheepId",
                table: "Cheeps",
                column: "CheepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Cheeps_CheepId",
                table: "Cheeps",
                column: "CheepId",
                principalTable: "Cheeps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Cheeps_CheepId",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_CheepId",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "CheepId",
                table: "Cheeps");
        }
    }
}
