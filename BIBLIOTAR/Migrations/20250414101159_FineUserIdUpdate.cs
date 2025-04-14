using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class FineUserIdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Users_UserId1",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_UserId1",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Fines");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Fines",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_UserId",
                table: "Fines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Users_UserId",
                table: "Fines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Users_UserId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_UserId",
                table: "Fines");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Fines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Fines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_UserId1",
                table: "Fines",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Users_UserId1",
                table: "Fines",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
