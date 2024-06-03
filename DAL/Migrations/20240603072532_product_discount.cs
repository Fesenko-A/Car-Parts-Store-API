using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class product_discount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPercentage",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "Products",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bosch" },
                    { 2, "Castrol" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Oil" },
                    { 2, "Batteries" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "Description" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "Pay online" }
                });

            migrationBuilder.InsertData(
                table: "SpecialTag",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "" },
                    { 2, "Best Seller" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "InStock", "Name", "Price", "SpecialTagId" },
                values: new object[,]
                {
                    { 1, 2, 1, "Non-stop protection from every start.", "https://images.lteplatform.com/images/products/600x600/521771951.jpg", true, "Castrol Magnatec Engine Oil - 10W-40 - 4ltr", 34.740000000000002, 2 },
                    { 2, 1, 2, "Bosch S4 car batteries are a high quality, premium replacement for you original car battery.", "https://images.lteplatform.com/images/products/600x600/444770757.jpg", true, "Bosch Car Battery 075", 85.890000000000001, 1 }
                });
        }
    }
}
