using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeBusinessIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // جعل BusinessId في AspNetUsers اختياري (nullable)
            migrationBuilder.AlterColumn<string>(
                name: "BusinessId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إرجاع BusinessId إلى مطلوب (NOT NULL) - تحذير: قد يفشل إذا كان هناك مستخدمين بدون BusinessId
            migrationBuilder.AlterColumn<string>(
                name: "BusinessId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

