using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class FineCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
