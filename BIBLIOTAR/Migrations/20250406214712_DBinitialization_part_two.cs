using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTar.Migrations
{
    /// <inheritdoc />
    public partial class DBinitialization_part_two : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UserEmail",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_Email",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserEmail",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UserEmail",
                table: "Borrows",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_Email",
                table: "Reservations",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserEmail",
                table: "UserRoles",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UserEmail",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_Email",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserEmail",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UserEmail",
                table: "Borrows",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Borrows_BorrowId",
                table: "Fines",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_Email",
                table: "Reservations",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserEmail",
                table: "UserRoles",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
