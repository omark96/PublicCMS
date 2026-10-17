using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityToNavMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminOnly",
                schema: "website",
                table: "NavMenuItems");

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                schema: "website",
                table: "NavMenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                schema: "website",
                table: "NavMenuItems");

            migrationBuilder.AddColumn<bool>(
                name: "AdminOnly",
                schema: "website",
                table: "NavMenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
