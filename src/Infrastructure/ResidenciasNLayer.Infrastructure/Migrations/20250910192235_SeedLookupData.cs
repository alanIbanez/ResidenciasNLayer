using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResidenciasNLayer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedLookupData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Roles
            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Preceptor" },
                    { "Tutor" },
                    { "Guardia" },
                    { "Residente" }
                });

            // Seed PreceptorTypes
            migrationBuilder.InsertData(
                table: "preceptortype",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Administrador" },
                    { "Monitor" }
                });

            // Seed ResidentTypes
            migrationBuilder.InsertData(
                table: "residenttype",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "universitario" },
                    { "colegio" }
                });

            // Seed ExitTypes
            migrationBuilder.InsertData(
                table: "exittype",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "casual" },
                    { "especial" }
                });

            // Seed ExitStatuses
            migrationBuilder.InsertData(
                table: "exitstatus",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "solicitado" },
                    { "en_proceso" },
                    { "autorizacion_tutor" },
                    { "autorizacion_preceptor" },
                    { "autorizado" },
                    { "rechazado" },
                    { "cancelado" }
                });

            // Seed Shifts
            migrationBuilder.InsertData(
                table: "shift",
                columns: new[] { "Name", "StartTime", "EndTime" },
                values: new object[,]
                {
                    { "Mañana", "06:00:00", "14:00:00" },
                    { "Tarde", "14:00:00", "22:00:00" },
                    { "Noche", "22:00:00", "06:00:00" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seed data
            migrationBuilder.DeleteData(table: "shift", keyColumn: "Name", keyValues: new object[] { "Mañana", "Tarde", "Noche" });
            migrationBuilder.DeleteData(table: "exitstatus", keyColumn: "Name", keyValues: new object[] { "solicitado", "en_proceso", "autorizacion_tutor", "autorizacion_preceptor", "autorizado", "rechazado", "cancelado" });
            migrationBuilder.DeleteData(table: "exittype", keyColumn: "Name", keyValues: new object[] { "casual", "especial" });
            migrationBuilder.DeleteData(table: "residenttype", keyColumn: "Name", keyValues: new object[] { "universitario", "colegio" });
            migrationBuilder.DeleteData(table: "preceptortype", keyColumn: "Name", keyValues: new object[] { "Administrador", "Monitor" });
            migrationBuilder.DeleteData(table: "role", keyColumn: "Name", keyValues: new object[] { "Preceptor", "Tutor", "Guardia", "Residente" });
        }
    }
}
