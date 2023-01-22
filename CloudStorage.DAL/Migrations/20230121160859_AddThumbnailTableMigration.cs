using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CloudStorage.DAL.Migrations
{
    public partial class AddThumbnailTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    UploadedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDescription", x => x.FileDescriptionId);
                });

            migrationBuilder.CreateTable(
                name: "ThumbnailInfoDbModel",
                columns: table => new
                {
                    ThumbnailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueName = table.Column<string>(type: "text", nullable: true),
                    FileDescriptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThumbnailInfoDbModel", x => x.ThumbnailId);
                    table.ForeignKey(
                        name: "FK_ThumbnailInfoDbModel_FileDescription_FileDescriptionId",
                        column: x => x.FileDescriptionId,
                        principalTable: "FileDescription",
                        principalColumn: "FileDescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThumbnailInfoDbModel_FileDescriptionId",
                table: "ThumbnailInfoDbModel",
                column: "FileDescriptionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThumbnailInfoDbModel");

            migrationBuilder.DropTable(
                name: "FileDescription");
        }
    }
}
