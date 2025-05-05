using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class Investigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReportUpvotes_ReportPostId",
                table: "ReportUpvotes");

            migrationBuilder.CreateTable(
                name: "AuthorViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportStatusViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatusViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LoggedInUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationViewModel_AuthorViewModel_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AuthorViewModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationViewModel_CategoryViewModel_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestigationViewModel_ReportStatusViewModel_ReportStatusId",
                        column: x => x.ReportStatusId,
                        principalTable: "ReportStatusViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvotes_ReportPostId_UserId",
                table: "ReportUpvotes",
                columns: new[] { "ReportPostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationViewModel_AuthorId",
                table: "InvestigationViewModel",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationViewModel_CategoryId",
                table: "InvestigationViewModel",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationViewModel_ReportStatusId",
                table: "InvestigationViewModel",
                column: "ReportStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestigationViewModel");

            migrationBuilder.DropTable(
                name: "AuthorViewModel");

            migrationBuilder.DropTable(
                name: "CategoryViewModel");

            migrationBuilder.DropTable(
                name: "ReportStatusViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ReportUpvotes_ReportPostId_UserId",
                table: "ReportUpvotes");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvotes_ReportPostId",
                table: "ReportUpvotes",
                column: "ReportPostId");
        }
    }
}
