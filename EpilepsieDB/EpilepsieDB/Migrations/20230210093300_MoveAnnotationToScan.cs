using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpilepsieDB.Migrations
{
    public partial class MoveAnnotationToScan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations");

            migrationBuilder.DropColumn(
                name: "IsOnset",
                table: "Annotations");

            migrationBuilder.AlterColumn<int>(
                name: "BlockID",
                table: "Annotations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ScanID",
                table: "Annotations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_ScanID",
                table: "Annotations",
                column: "ScanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations",
                column: "BlockID",
                principalTable: "Blocks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotations_Scans_ScanID",
                table: "Annotations",
                column: "ScanID",
                principalTable: "Scans",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Annotations_Scans_ScanID",
                table: "Annotations");

            migrationBuilder.DropIndex(
                name: "IX_Annotations_ScanID",
                table: "Annotations");

            migrationBuilder.DropColumn(
                name: "ScanID",
                table: "Annotations");

            migrationBuilder.AlterColumn<int>(
                name: "BlockID",
                table: "Annotations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnset",
                table: "Annotations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Annotations_Blocks_BlockID",
                table: "Annotations",
                column: "BlockID",
                principalTable: "Blocks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
