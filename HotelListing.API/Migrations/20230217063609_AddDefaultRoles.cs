using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e1b2bbbb-6e55-45f5-87de-0767829d14c1", "cf7094ec-8818-46eb-8fd8-424f4ef2a056", "Administrator", "ADMINISTRATOR" },
                    { "e62a2c72-5cd7-4852-924f-4426a9e9eaa7", "c6fbb6fe-357a-415f-b0ff-c29b669e3182", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1b2bbbb-6e55-45f5-87de-0767829d14c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e62a2c72-5cd7-4852-924f-4426a9e9eaa7");
        }
    }
}
