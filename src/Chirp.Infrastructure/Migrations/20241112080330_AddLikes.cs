using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheepLike",
                columns: table => new
                {
                    LikedCheepsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikesId = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheepLike", x => new { x.LikedCheepsId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_CheepLike_AspNetUsers_LikesId",
                        column: x => x.LikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheepLike_Cheeps_LikedCheepsId",
                        column: x => x.LikedCheepsId,
                        principalTable: "Cheeps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheepLike_LikesId",
                table: "CheepLike",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheepLike");
        }
    }
}
