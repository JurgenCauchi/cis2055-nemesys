using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class Investigation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestigationViewModel_CategoryViewModel_CategoryId",
                table: "InvestigationViewModel");

            migrationBuilder.DropIndex(
                name: "IX_InvestigationViewModel_CategoryId",
                table: "InvestigationViewModel");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "InvestigationViewModel");

            migrationBuilder.CreateTable(
                name: "HazardTypeViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardTypeViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investigations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportStatusId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    HazardTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investigations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Investigations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Investigations_HazardTypes_HazardTypeId",
                        column: x => x.HazardTypeId,
                        principalTable: "HazardTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Investigations_ReportStatuses_ReportStatusId",
                        column: x => x.ReportStatusId,
                        principalTable: "ReportStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportPostViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadCount = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LoggedInUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpvoteCount = table.Column<int>(type: "int", nullable: false),
                    HasUpvoted = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HazardTypeId = table.Column<int>(type: "int", nullable: false),
                    ReportStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPostViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportPostViewModel_AuthorViewModel_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AuthorViewModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportPostViewModel_CategoryViewModel_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportPostViewModel_HazardTypeViewModel_HazardTypeId",
                        column: x => x.HazardTypeId,
                        principalTable: "HazardTypeViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportPostViewModel_ReportStatusViewModel_ReportStatusId",
                        column: x => x.ReportStatusId,
                        principalTable: "ReportStatusViewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_CategoryId",
                table: "Investigations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_HazardTypeId",
                table: "Investigations",
                column: "HazardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_ReportStatusId",
                table: "Investigations",
                column: "ReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_UserId",
                table: "Investigations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPostViewModel_AuthorId",
                table: "ReportPostViewModel",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPostViewModel_CategoryId",
                table: "ReportPostViewModel",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPostViewModel_HazardTypeId",
                table: "ReportPostViewModel",
                column: "HazardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPostViewModel_ReportStatusId",
                table: "ReportPostViewModel",
                column: "ReportStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investigations");

            migrationBuilder.DropTable(
                name: "ReportPostViewModel");

            migrationBuilder.DropTable(
                name: "HazardTypeViewModel");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "InvestigationViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationViewModel_CategoryId",
                table: "InvestigationViewModel",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestigationViewModel_CategoryViewModel_CategoryId",
                table: "InvestigationViewModel",
                column: "CategoryId",
                principalTable: "CategoryViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
