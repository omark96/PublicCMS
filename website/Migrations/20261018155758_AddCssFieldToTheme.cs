using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddCssFieldToTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Css",
                schema: "website",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Css",
                schema: "website",
                table: "Themes");
        }
    }
}
