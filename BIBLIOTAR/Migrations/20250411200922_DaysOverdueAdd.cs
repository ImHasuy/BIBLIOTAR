using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class DaysOverdueAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysOverdue",
                table: "Fines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOverdue",
                table: "Fines");
        }
    }
}
