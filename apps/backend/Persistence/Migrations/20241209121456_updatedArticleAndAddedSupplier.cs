using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedArticleAndAddedSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Articles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SupplierId",
                table: "Articles",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Suppliers_SupplierId",
                table: "Articles",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Suppliers_SupplierId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SupplierId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Articles");
        }
    }
}
