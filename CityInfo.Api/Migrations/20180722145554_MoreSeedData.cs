using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfo.Api.Migrations
{
    public partial class MoreSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 500, "Capital of UK", "London" });

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 2, 500, "Parliement and stuff", "Westminister" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 500);
        }
    }
}
