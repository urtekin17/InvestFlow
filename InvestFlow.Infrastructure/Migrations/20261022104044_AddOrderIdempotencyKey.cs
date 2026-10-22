using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdempotencyKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Önce nullable sütun ekle
            migrationBuilder.AddColumn<Guid>(
                name: "IdempotencyKey",
                table: "Orders",
                type: "uuid",
                nullable: true);

            // 2. Mevcut her Order için benzersiz UUID oluştur
            migrationBuilder.Sql("""
            UPDATE "Orders"
            SET "IdempotencyKey" = gen_random_uuid()
            WHERE "IdempotencyKey" IS NULL;
            """);

            // 3. Sütunu zorunlu hale getir
            migrationBuilder.AlterColumn<Guid>(
                name: "IdempotencyKey",
                table: "Orders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            // 4. Unique Index oluştur
            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdempotencyKey",
                table: "Orders",
                column: "IdempotencyKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_IdempotencyKey",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdempotencyKey",
                table: "Orders");
        }
    }
}
