using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultThemeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                schema: "website",
                table: "Themes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                schema: "website",
                table: "Themes");
        }
    }
}
