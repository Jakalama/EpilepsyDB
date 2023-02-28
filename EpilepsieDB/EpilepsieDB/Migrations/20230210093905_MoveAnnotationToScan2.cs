using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpilepsieDB.Migrations
{
    public partial class MoveAnnotationToScan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations");

            migrationBuilder.DropIndex(
                name: "IX_Annotations_BlockID",
                table: "Annotations");

            migrationBuilder.DropColumn(
                name: "BlockID",
                table: "Annotations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockID",
                table: "Annotations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_BlockID",
                table: "Annotations",
                column: "BlockID");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations",
                column: "BlockID",
                principalTable: "Blocks",
                principalColumn: "ID");
        }
    }
}
