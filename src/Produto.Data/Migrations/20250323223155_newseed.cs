using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Produto.Data.Migrations
{
    /// <inheritdoc />
    public partial class newseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "Id", "Descricao", "Estoque", "Nome", "Valor" },
                values: new object[,]
                {
                    { new Guid("0195c525-9a61-711c-8f23-fe5648bb690a"), "Descrição do Produto 1", 100, "Produto 1", 10.00m },
                    { new Guid("0195c525-9a61-711c-8f24-03f9b0e3cd25"), "Descrição do Produto 2", 200, "Produto 2", 20.00m },
                    { new Guid("0195c525-9a61-711c-8f24-0592096c4b61"), "Descrição do Produto 3", 300, "Produto 3", 30.00m },
                    { new Guid("0195c525-9a61-711c-8f24-0b3d685cc7d6"), "Descrição do Produto 4", 400, "Produto 4", 40.00m },
                    { new Guid("0195c525-9a61-711c-8f24-0c11233469cf"), "Descrição do Produto 5", 500, "Produto 5", 50.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: new Guid("0195c525-9a61-711c-8f23-fe5648bb690a"));

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: new Guid("0195c525-9a61-711c-8f24-03f9b0e3cd25"));

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: new Guid("0195c525-9a61-711c-8f24-0592096c4b61"));

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: new Guid("0195c525-9a61-711c-8f24-0b3d685cc7d6"));

            migrationBuilder.DeleteData(
                table: "produtos",
                keyColumn: "Id",
                keyValue: new Guid("0195c525-9a61-711c-8f24-0c11233469cf"));
        }
    }
}
