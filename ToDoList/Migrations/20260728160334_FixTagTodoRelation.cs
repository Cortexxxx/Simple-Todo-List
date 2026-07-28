using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class FixTagTodoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTodoItem_Todos_TodoItemsId",
                table: "TagTodoItem");

            migrationBuilder.RenameColumn(
                name: "TodoItemsId",
                table: "TagTodoItem",
                newName: "TodoItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TagTodoItem_TodoItemsId",
                table: "TagTodoItem",
                newName: "IX_TagTodoItem_TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTodoItem_Todos_TodoItemId",
                table: "TagTodoItem",
                column: "TodoItemId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTodoItem_Todos_TodoItemId",
                table: "TagTodoItem");

            migrationBuilder.RenameColumn(
                name: "TodoItemId",
                table: "TagTodoItem",
                newName: "TodoItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_TagTodoItem_TodoItemId",
                table: "TagTodoItem",
                newName: "IX_TagTodoItem_TodoItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTodoItem_Todos_TodoItemsId",
                table: "TagTodoItem",
                column: "TodoItemsId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
