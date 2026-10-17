using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS_API.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryImage_Galleries_GalleryId",
                schema: "api",
                table: "GalleryImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GalleryImage",
                schema: "api",
                table: "GalleryImage");

            migrationBuilder.RenameTable(
                name: "GalleryImage",
                schema: "api",
                newName: "Images",
                newSchema: "api");

            migrationBuilder.RenameColumn(
                name: "OriginalName",
                schema: "api",
                table: "Images",
                newName: "Path");

            migrationBuilder.RenameIndex(
                name: "IX_GalleryImage_GalleryId",
                schema: "api",
                table: "Images",
                newName: "IX_Images_GalleryId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "api",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                schema: "api",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                schema: "api",
                table: "Images",
                column: "GalleryId",
                principalSchema: "api",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                schema: "api",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                schema: "api",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "api",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                schema: "api",
                newName: "GalleryImage",
                newSchema: "api");

            migrationBuilder.RenameColumn(
                name: "Path",
                schema: "api",
                table: "GalleryImage",
                newName: "OriginalName");

            migrationBuilder.RenameIndex(
                name: "IX_Images_GalleryId",
                schema: "api",
                table: "GalleryImage",
                newName: "IX_GalleryImage_GalleryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GalleryImage",
                schema: "api",
                table: "GalleryImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryImage_Galleries_GalleryId",
                schema: "api",
                table: "GalleryImage",
                column: "GalleryId",
                principalSchema: "api",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
