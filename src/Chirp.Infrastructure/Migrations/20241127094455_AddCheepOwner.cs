using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheepOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Cheeps_CheepId",
                table: "Cheeps");

            migrationBuilder.RenameColumn(
                name: "CheepId",
                table: "Cheeps",
                newName: "CheepOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_CheepId",
                table: "Cheeps",
                newName: "IX_Cheeps_CheepOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Cheeps_CheepOwnerId",
                table: "Cheeps",
                column: "CheepOwnerId",
                principalTable: "Cheeps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Cheeps_CheepOwnerId",
                table: "Cheeps");

            migrationBuilder.RenameColumn(
                name: "CheepOwnerId",
                table: "Cheeps",
                newName: "CheepId");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_CheepOwnerId",
                table: "Cheeps",
                newName: "IX_Cheeps_CheepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Cheeps_CheepId",
                table: "Cheeps",
                column: "CheepId",
                principalTable: "Cheeps",
                principalColumn: "Id");
        }
    }
}
