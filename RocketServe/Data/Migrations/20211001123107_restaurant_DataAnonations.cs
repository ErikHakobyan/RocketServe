using Microsoft.EntityFrameworkCore.Migrations;

namespace RocketServe.Data.Migrations
{
    public partial class restaurant_DataAnonations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a2deed58-481b-4369-9dfb-4b9eebd7be20", "51f159c4-ec2d-40cc-a897-199ba9b51ef6", "admin", "ADMIN" },
                    { "6122400f-6c2f-4a68-85e1-907004bcf45b", "e250da01-cd47-441b-9ddf-048eed630761", "restaurant_admin", "RESTAURANT_ADMIN" },
                    { "2b7fac92-f8de-4bbd-b736-e4f2d2a8b57d", "19fb657c-4728-46af-829c-7bad51cb1ae3", "manager", "MANAGER" },
                    { "221aa1a6-f4ac-4b6c-bf8c-53cb78533c9b", "c1a43c58-6107-426b-891f-2366696df9fc", "worker", "WORKER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "221aa1a6-f4ac-4b6c-bf8c-53cb78533c9b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b7fac92-f8de-4bbd-b736-e4f2d2a8b57d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6122400f-6c2f-4a68-85e1-907004bcf45b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2deed58-481b-4369-9dfb-4b9eebd7be20");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
