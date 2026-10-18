using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddThemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Themes",
                schema: "website",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeaderCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParagraphCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkCss = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Themes",
                schema: "website");
        }
    }
}
