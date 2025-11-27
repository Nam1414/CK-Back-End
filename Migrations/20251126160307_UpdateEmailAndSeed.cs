using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "PasswordHash" },
                values: new object[] { "admin@gmail.com", "Quản Trị Viên", "$2a$11$XQ3JmtZFtKX/tvONNilvEeF0JRxQThh273bhNHqNzHiQV/kOLdvmW" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "PasswordHash" },
                values: new object[] { "", "Administrator", "$2a$11$aoQDdIiTLOtzd1zTG/uI4ugDflE15izJrihyhl7A4IGf3N/rBq5MS" });
        }
    }
}
