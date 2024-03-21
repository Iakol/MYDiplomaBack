using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class DebugCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardItems_shopingCards_ShopingCardId",
                table: "CardItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCardId",
                table: "CardItems");

            migrationBuilder.AlterColumn<int>(
                name: "ShopingCardId",
                table: "CardItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardItems_shopingCards_ShopingCardId",
                table: "CardItems",
                column: "ShopingCardId",
                principalTable: "shopingCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardItems_shopingCards_ShopingCardId",
                table: "CardItems");

            migrationBuilder.AlterColumn<int>(
                name: "ShopingCardId",
                table: "CardItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCardId",
                table: "CardItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CardItems_shopingCards_ShopingCardId",
                table: "CardItems",
                column: "ShopingCardId",
                principalTable: "shopingCards",
                principalColumn: "Id");
        }
    }
}
