using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshop_api.Migrations
{
    public partial class fixValidattion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Check",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Phone1",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "Phone2",
                table: "AppUsers",
                newName: "Phone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckedAt",
                table: "Orders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Orders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "AppUsers",
                newName: "Phone2");

            migrationBuilder.AlterColumn<string>(
                name: "CheckedAt",
                table: "Orders",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Check",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone1",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
