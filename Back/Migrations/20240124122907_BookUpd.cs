using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class BookUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EBookCost",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "PaperBookCost",
                table: "Books",
                newName: "BookCost");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Books",
                type: "integer",
                nullable: true,
                defaultValue: null,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedBookId",
                table: "Books",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_RelatedBookId",
                table: "Books",
                column: "RelatedBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_RelatedBookId",
                table: "Books",
                column: "RelatedBookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_RelatedBookId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_RelatedBookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RelatedBookId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookCost",
                table: "Books",
                newName: "PaperBookCost");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Books",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "EBookCost",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
