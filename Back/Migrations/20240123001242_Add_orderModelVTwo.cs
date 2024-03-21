using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class Add_orderModelVTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_CardItems_CardItemId",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "CardItemId",
                table: "orderDetails",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_CardItemId",
                table: "orderDetails",
                newName: "IX_orderDetails_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_Books_BookId",
                table: "orderDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_Books_BookId",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "orderDetails",
                newName: "CardItemId");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_BookId",
                table: "orderDetails",
                newName: "IX_orderDetails_CardItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_CardItems_CardItemId",
                table: "orderDetails",
                column: "CardItemId",
                principalTable: "CardItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
