using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId",
                unique: true);
        }
    }
}
