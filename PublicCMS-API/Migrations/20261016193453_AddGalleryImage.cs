using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalleryImage",
                schema: "api",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GalleryImage_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalSchema: "api",
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImage_GalleryId",
                schema: "api",
                table: "GalleryImage",
                column: "GalleryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryImage",
                schema: "api");
        }
    }
}
