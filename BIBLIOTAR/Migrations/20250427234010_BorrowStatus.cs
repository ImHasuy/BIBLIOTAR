using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class BorrowStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "borrowStatus",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "borrowStatus",
                table: "Borrows");
        }
    }
}
