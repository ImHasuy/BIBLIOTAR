using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class DaysOverdueRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOverdue",
                table: "Fines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysOverdue",
                table: "Fines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
