using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skwela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedDefaultTextCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "enrolled_status",
                table: "Enrollments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enrolled_status",
                table: "Enrollments");
        }
    }
}
