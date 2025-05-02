using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class FullReportData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HazardTypeId",
                table: "ReportPosts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ReportPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReportStatusId",
                table: "ReportPosts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "HazardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUpvotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportPostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUpvotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportUpvotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportUpvotes_ReportPosts_ReportPostId",
                        column: x => x.ReportPostId,
                        principalTable: "ReportPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HazardTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Unsafe Act" },
                    { 2, "Unsafe Condition" },
                    { 3, "Equipment Issue" },
                    { 4, "Unsafe Structure" }
                });

            migrationBuilder.InsertData(
                table: "ReportStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Being Investigated" },
                    { 3, "Investigation Complete" },
                    { 4, "Action Taken" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportPosts_HazardTypeId",
                table: "ReportPosts",
                column: "HazardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPosts_ReportStatusId",
                table: "ReportPosts",
                column: "ReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvotes_ReportPostId",
                table: "ReportUpvotes",
                column: "ReportPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUpvotes_UserId",
                table: "ReportUpvotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPosts_HazardTypes_HazardTypeId",
                table: "ReportPosts",
                column: "HazardTypeId",
                principalTable: "HazardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPosts_ReportStatuses_ReportStatusId",
                table: "ReportPosts",
                column: "ReportStatusId",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportPosts_HazardTypes_HazardTypeId",
                table: "ReportPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportPosts_ReportStatuses_ReportStatusId",
                table: "ReportPosts");

            migrationBuilder.DropTable(
                name: "HazardTypes");

            migrationBuilder.DropTable(
                name: "ReportStatuses");

            migrationBuilder.DropTable(
                name: "ReportUpvotes");

            migrationBuilder.DropIndex(
                name: "IX_ReportPosts_HazardTypeId",
                table: "ReportPosts");

            migrationBuilder.DropIndex(
                name: "IX_ReportPosts_ReportStatusId",
                table: "ReportPosts");

            migrationBuilder.DropColumn(
                name: "HazardTypeId",
                table: "ReportPosts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ReportPosts");

            migrationBuilder.DropColumn(
                name: "ReportStatusId",
                table: "ReportPosts");
        }
    }
}
