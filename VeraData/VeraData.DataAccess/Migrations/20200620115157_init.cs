using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VeraData.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cwes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RemediationEffort = table.Column<int>(nullable: false),
                    Exploitability = table.Column<int>(nullable: false),
                    Pci = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cwes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MitigationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MitigationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RemediationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemediationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Severities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Severities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadFileStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFileStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sandboxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sandboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sandboxes_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<int>(nullable: false),
                    Submitter = table.Column<string>(nullable: true),
                    ScanStatus = table.Column<string>(nullable: true),
                    ScanType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SandboxId = table.Column<int>(nullable: true),
                    ScanStart = table.Column<DateTime>(nullable: false),
                    ScanEnd = table.Column<DateTime>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scans_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scans_Sandboxes_SandboxId",
                        column: x => x.SandboxId,
                        principalTable: "Sandboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<long>(nullable: false),
                    Size = table.Column<double>(nullable: false),
                    Hash = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    EntryPoint = table.Column<bool>(nullable: false),
                    AddedId = table.Column<int>(nullable: true),
                    ModifiedId = table.Column<int>(nullable: true),
                    RemovedId = table.Column<int>(nullable: true),
                    ScanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_ModuleFile_AddedId",
                        column: x => x.AddedId,
                        principalTable: "ModuleFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_ModuleFile_ModifiedId",
                        column: x => x.ModifiedId,
                        principalTable: "ModuleFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_ModuleFile_RemovedId",
                        column: x => x.RemovedId,
                        principalTable: "ModuleFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_Scans_ScanId",
                        column: x => x.ScanId,
                        principalTable: "Scans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    ScanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadFiles_Scans_ScanId",
                        column: x => x.ScanId,
                        principalTable: "Scans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadFiles_UploadFileStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "UploadFileStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreScanError",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(nullable: true),
                    Error = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreScanError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreScanError_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceFiles_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flaws",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeracodeId = table.Column<int>(nullable: false),
                    SeverityId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    VeracodeCategoryId = table.Column<int>(nullable: false),
                    CweId = table.Column<int>(nullable: true),
                    VeracodeCweId = table.Column<int>(nullable: false),
                    RemediationStatus = table.Column<string>(nullable: true),
                    LineNumber = table.Column<int>(nullable: false),
                    PrototypeFunction = table.Column<string>(nullable: true),
                    Function = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    SourceFileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flaws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flaws_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flaws_Cwes_CweId",
                        column: x => x.CweId,
                        principalTable: "Cwes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flaws_Severities_SeverityId",
                        column: x => x.SeverityId,
                        principalTable: "Severities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flaws_SourceFiles_SourceFileId",
                        column: x => x.SourceFileId,
                        principalTable: "SourceFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Severities",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Informational" },
                    { 2, "Low" },
                    { 3, "Medium" },
                    { 4, "High" },
                    { 5, "Very High" }
                });

            migrationBuilder.InsertData(
                table: "UploadFileStatuses",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "PendingUpload" },
                    { 2, "Uploading" },
                    { 3, "Purged" },
                    { 4, "Uploaded" },
                    { 5, "Missing" },
                    { 6, "Partial" },
                    { 7, "InvalidChecksum" },
                    { 8, "InvalidArchive" },
                    { 9, "ArchiveFileWithinAnotherArchive" },
                    { 10, "ArchiveFilewithUnsupportedCompression" },
                    { 11, "ArchiveFileisPasswordProtected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flaws_CategoryId",
                table: "Flaws",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Flaws_CweId",
                table: "Flaws",
                column: "CweId");

            migrationBuilder.CreateIndex(
                name: "IX_Flaws_SeverityId",
                table: "Flaws",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_Flaws_SourceFileId",
                table: "Flaws",
                column: "SourceFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_AddedId",
                table: "Modules",
                column: "AddedId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ModifiedId",
                table: "Modules",
                column: "ModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_RemovedId",
                table: "Modules",
                column: "RemovedId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ScanId",
                table: "Modules",
                column: "ScanId");

            migrationBuilder.CreateIndex(
                name: "IX_PreScanError_ModuleId",
                table: "PreScanError",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sandboxes_ApplicationId",
                table: "Sandboxes",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Scans_ApplicationId",
                table: "Scans",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Scans_SandboxId",
                table: "Scans",
                column: "SandboxId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceFiles_ModuleId",
                table: "SourceFiles",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_ScanId",
                table: "UploadFiles",
                column: "ScanId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_StatusId",
                table: "UploadFiles",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flaws");

            migrationBuilder.DropTable(
                name: "MitigationStatuses");

            migrationBuilder.DropTable(
                name: "PreScanError");

            migrationBuilder.DropTable(
                name: "RemediationStatuses");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Cwes");

            migrationBuilder.DropTable(
                name: "Severities");

            migrationBuilder.DropTable(
                name: "SourceFiles");

            migrationBuilder.DropTable(
                name: "UploadFileStatuses");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "ModuleFile");

            migrationBuilder.DropTable(
                name: "Scans");

            migrationBuilder.DropTable(
                name: "Sandboxes");

            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
