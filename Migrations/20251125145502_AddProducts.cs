using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "Laptop mỏng nhẹ cao cấp", "Laptop Dell XPS 13", 25000000m, 10 },
                    { 2, "Titan tự nhiên, 256GB", "iPhone 15 Pro", 28000000m, 15 },
                    { 3, "AI Phone mới nhất", "Samsung Galaxy S24", 22000000m, 20 },
                    { 4, "Tai nghe chống ồn chủ động", "AirPods Pro 2", 6000000m, 50 },
                    { 5, "Máy tính bảng hiệu năng cao", "iPad Air M1", 15000000m, 12 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$5zv90RdPonZvL6vAYeSSwuO4mBe/WgrMVlInyLPdI4l080dX0VHhy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$k2ukdInoubwbt.22g7F6ruvRqI6DVXI.RzJyxpmX2VdC9ytyiNAQS");
        }
    }
}
