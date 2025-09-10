using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ResidenciasNLayer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exitstatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exitstatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exittype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exittype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "preceptortype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preceptortype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "residenttype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_residenttype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PushToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "guard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ShiftId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_guard_shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_guard_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "preceptor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PreceptorTypeId = table.Column<int>(type: "integer", nullable: false),
                    ShiftId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preceptor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_preceptor_preceptortype_PreceptorTypeId",
                        column: x => x.PreceptorTypeId,
                        principalTable: "preceptortype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_preceptor_shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_preceptor_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tutor_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resident",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Institution = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ResidentTypeId = table.Column<int>(type: "integer", nullable: false),
                    TutorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resident_residenttype_ResidentTypeId",
                        column: x => x.ResidentTypeId,
                        principalTable: "residenttype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_resident_tutor_TutorId",
                        column: x => x.TutorId,
                        principalTable: "tutor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_resident_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attendance_resident_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "resident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlannedDepartureAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlannedReturnAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualDepartureAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualReturnAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    ExitTypeId = table.Column<int>(type: "integer", nullable: false),
                    ExitStatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exit_exitstatus_ExitStatusId",
                        column: x => x.ExitStatusId,
                        principalTable: "exitstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exit_exittype_ExitTypeId",
                        column: x => x.ExitTypeId,
                        principalTable: "exittype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exit_resident_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "resident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exitauthorization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PerformedByRole = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExitId = table.Column<int>(type: "integer", nullable: false),
                    PerformedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exitauthorization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exitauthorization_exit_ExitId",
                        column: x => x.ExitId,
                        principalTable: "exit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exitauthorization_users_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProviderMessageId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ExitId = table.Column<int>(type: "integer", nullable: true),
                    EventId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notification_exit_ExitId",
                        column: x => x.ExitId,
                        principalTable: "exit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notification_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_ResidentId_OccurredAt",
                table: "attendance",
                columns: new[] { "ResidentId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_Type",
                table: "attendance",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_exit_ExitStatusId",
                table: "exit",
                column: "ExitStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_exit_ExitTypeId",
                table: "exit",
                column: "ExitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_exit_ResidentId",
                table: "exit",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_exitauthorization_ExitId_Action",
                table: "exitauthorization",
                columns: new[] { "ExitId", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_exitauthorization_PerformedByUserId",
                table: "exitauthorization",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_exitstatus_Name",
                table: "exitstatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exittype_Name",
                table: "exittype",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_guard_ShiftId",
                table: "guard",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_guard_UserId",
                table: "guard",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_ExitId",
                table: "notification",
                column: "ExitId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_Type",
                table: "notification",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_notification_UserId_IsRead",
                table: "notification",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_preceptor_PreceptorTypeId",
                table: "preceptor",
                column: "PreceptorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_preceptor_ShiftId",
                table: "preceptor",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_preceptor_UserId",
                table: "preceptor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_preceptortype_Name",
                table: "preceptortype",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_resident_ResidentTypeId",
                table: "resident",
                column: "ResidentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_resident_StudentId",
                table: "resident",
                column: "StudentId",
                unique: true,
                filter: "\"StudentId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_resident_TutorId",
                table: "resident",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_resident_UserId",
                table: "resident",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_residenttype_Name",
                table: "residenttype",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_Name",
                table: "role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shift_Name",
                table: "shift",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_UserId",
                table: "tutor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance");

            migrationBuilder.DropTable(
                name: "exitauthorization");

            migrationBuilder.DropTable(
                name: "guard");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "preceptor");

            migrationBuilder.DropTable(
                name: "exit");

            migrationBuilder.DropTable(
                name: "preceptortype");

            migrationBuilder.DropTable(
                name: "shift");

            migrationBuilder.DropTable(
                name: "exitstatus");

            migrationBuilder.DropTable(
                name: "exittype");

            migrationBuilder.DropTable(
                name: "resident");

            migrationBuilder.DropTable(
                name: "residenttype");

            migrationBuilder.DropTable(
                name: "tutor");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
