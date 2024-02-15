using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class onlinepayments_userid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OnlinePayments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePayments_UserId",
                table: "OnlinePayments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayments_AspNetUsers_UserId",
                table: "OnlinePayments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayments_AspNetUsers_UserId",
                table: "OnlinePayments");

            migrationBuilder.DropIndex(
                name: "IX_OnlinePayments_UserId",
                table: "OnlinePayments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OnlinePayments");
        }
    }
}
