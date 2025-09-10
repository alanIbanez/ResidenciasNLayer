using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidenciasNLayer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderResponseToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderResponse",
                table: "notification",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderResponse",
                table: "notification");
        }
    }
}
