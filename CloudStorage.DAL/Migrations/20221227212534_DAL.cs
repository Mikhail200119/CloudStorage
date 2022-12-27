using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CloudStorage.DAL.Migrations
{
    public partial class DAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileFolder",
                columns: table => new
                {
                    FileFolderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFolder", x => x.FileFolderId);
                    table.ForeignKey(
                        name: "FK_FileFolder_FileFolder_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "FileFolder",
                        principalColumn: "FileFolderId");
                });

            migrationBuilder.CreateTable(
                name: "FileDescription",
                columns: table => new
                {
                    FileDescriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProvidedName = table.Column<string>(type: "text", nullable: false),
                    UniqueName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    SizeInBytes = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContentHash = table.Column<string>(type: "text", nullable: false),
                    UploadedBy = table.Column<string>(type: "text", nullable: false),
                    Preview = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileFolderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDescription", x => x.FileDescriptionId);
                    table.ForeignKey(
                        name: "FK_FileDescription_FileFolder_FileFolderId",
                        column: x => x.FileFolderId,
                        principalTable: "FileFolder",
                        principalColumn: "FileFolderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDescription_FileFolderId",
                table: "FileDescription",
                column: "FileFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolder_ParentFolderId",
                table: "FileFolder",
                column: "ParentFolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDescription");

            migrationBuilder.DropTable(
                name: "FileFolder");
        }
    }
}
