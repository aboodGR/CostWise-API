using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CostWise_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryExpenseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expense_CategoryId",
                table: "Expense",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Category_CategoryId",
                table: "Expense",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Category_CategoryId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_CategoryId",
                table: "Expense");
        }
    }
}
