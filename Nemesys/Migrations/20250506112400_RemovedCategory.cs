using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportPosts_Categories_CategoryId",
                table: "ReportPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportPostViewModel_CategoryViewModel_CategoryId",
                table: "ReportPostViewModel");

            migrationBuilder.DropTable(
                name: "CategoryViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ReportPostViewModel_CategoryId",
                table: "ReportPostViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ReportPosts_CategoryId",
                table: "ReportPosts");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ReportPostViewModel");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ReportPosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ReportPostViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ReportPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Uncategorised" },
                    { 2, "Comedy" },
                    { 3, "News" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportPostViewModel_CategoryId",
                table: "ReportPostViewModel",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPosts_CategoryId",
                table: "ReportPosts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPosts_Categories_CategoryId",
                table: "ReportPosts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPostViewModel_CategoryViewModel_CategoryId",
                table: "ReportPostViewModel",
                column: "CategoryId",
                principalTable: "CategoryViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
