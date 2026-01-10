using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skwela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                newName: "refreshTokenExpiryTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "refreshTokenExpiryTime",
                table: "Users",
                newName: "RefreshTokenExpiryTime");
        }
    }
}
