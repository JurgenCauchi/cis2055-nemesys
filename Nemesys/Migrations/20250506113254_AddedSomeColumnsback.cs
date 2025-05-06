using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeColumnsback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
     name: "CategoryId",
     table: "Investigations",
     type: "int",
     nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HazardTypeId",
                table: "Investigations",
                type: "int",
                nullable: true);

            // Add foreign keys back
            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Categories_CategoryId",
                table: "Investigations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_HazardTypes_HazardTypeId",
                table: "Investigations",
                column: "HazardTypeId",
                principalTable: "HazardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_Categories_CategoryId",
                table: "Investigations");

            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_HazardTypes_HazardTypeId",
                table: "Investigations");

            migrationBuilder.AlterColumn<int>(
                name: "HazardTypeId",
                table: "Investigations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Investigations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_Categories_CategoryId",
                table: "Investigations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_HazardTypes_HazardTypeId",
                table: "Investigations",
                column: "HazardTypeId",
                principalTable: "HazardTypes",
                principalColumn: "Id");
        }
    }
}
