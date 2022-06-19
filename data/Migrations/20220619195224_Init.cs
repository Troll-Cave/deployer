using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using data.Models;

#nullable disable

namespace data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_config", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pipeline",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    org = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pipeline", x => x.id);
                    table.ForeignKey(
                        name: "FK_pipeline_organization_org",
                        column: x => x.org,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "pipeline_version",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    pipeline = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<Pipeline>(type: "jsonb", nullable: false),
                    files = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pipeline_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_pipeline_version_pipeline_pipeline",
                        column: x => x.pipeline,
                        principalTable: "pipeline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    pipeline = table.Column<Guid>(type: "uuid", nullable: true),
                    org = table.Column<Guid>(type: "uuid", nullable: true),
                    variables = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application", x => x.id);
                    table.ForeignKey(
                        name: "FK_application_organization_org",
                        column: x => x.org,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_application_pipeline_version_pipeline",
                        column: x => x.pipeline,
                        principalTable: "pipeline_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<Pipeline>(type: "jsonb", nullable: false),
                    metadata = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    step_state = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    pipeline = table.Column<Guid>(type: "uuid", nullable: true),
                    application = table.Column<Guid>(type: "uuid", nullable: false),
                    job_state = table.Column<string>(type: "text", nullable: false),
                    source_reference = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_application_application",
                        column: x => x.application,
                        principalTable: "application",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_pipeline_version_pipeline",
                        column: x => x.pipeline,
                        principalTable: "pipeline_version",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_application_org",
                table: "application",
                column: "org");

            migrationBuilder.CreateIndex(
                name: "IX_application_pipeline",
                table: "application",
                column: "pipeline");

            migrationBuilder.CreateIndex(
                name: "IX_job_application",
                table: "job",
                column: "application");

            migrationBuilder.CreateIndex(
                name: "IX_job_pipeline",
                table: "job",
                column: "pipeline");

            migrationBuilder.CreateIndex(
                name: "IX_pipeline_org",
                table: "pipeline",
                column: "org");

            migrationBuilder.CreateIndex(
                name: "IX_pipeline_version_pipeline",
                table: "pipeline_version",
                column: "pipeline");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "config");

            migrationBuilder.DropTable(
                name: "job");

            migrationBuilder.DropTable(
                name: "application");

            migrationBuilder.DropTable(
                name: "pipeline_version");

            migrationBuilder.DropTable(
                name: "pipeline");

            migrationBuilder.DropTable(
                name: "organization");
        }
    }
}
