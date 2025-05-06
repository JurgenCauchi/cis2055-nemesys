using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class Testing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_HazardTypes_HazardTypeId",
                table: "Investigations");

            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_Categories_CategoryId",
                table: "Investigations");

            // Drop the indexes that reference the columns
            migrationBuilder.DropIndex(
                name: "IX_Investigations_HazardTypeId",
                table: "Investigations");

            migrationBuilder.DropIndex(
                name: "IX_Investigations_CategoryId",
                table: "Investigations");

            // Now drop the columns
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "HazardTypeId",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Investigations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert changes in the Down method if you rollback the migration

            // Add columns back (as needed)
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Investigations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HazardTypeId",
                table: "Investigations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Investigations",
                type: "int",
                nullable: true);

            // Recreate the indexes
            migrationBuilder.CreateIndex(
                name: "IX_Investigations_HazardTypeId",
                table: "Investigations",
                column: "HazardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_CategoryId",
                table: "Investigations",
                column: "CategoryId");

            // Recreate the foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_HazardTypes_HazardTypeId",
                table: "Investigations",
                column: "HazardTypeId",
                principalTable: "HazardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Categories_CategoryId",
                table: "Investigations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }

}
