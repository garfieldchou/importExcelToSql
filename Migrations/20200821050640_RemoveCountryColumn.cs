using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class RemoveCountryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CountryNetworkDLStats");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CountryDLStatOverviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CountryNetworkDLStats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CountryDLStatOverviews",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
