using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class AddCountryIdAsForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryListId",
                table: "CountryNetworkDLStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryListId",
                table: "CountryDLStatOverviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CountryNetworkDLStats_CountryListId",
                table: "CountryNetworkDLStats",
                column: "CountryListId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryDLStatOverviews_CountryListId",
                table: "CountryDLStatOverviews",
                column: "CountryListId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryDLStatOverviews_CountryLists_CountryListId",
                table: "CountryDLStatOverviews",
                column: "CountryListId",
                principalTable: "CountryLists",
                principalColumn: "CountryListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CountryNetworkDLStats_CountryLists_CountryListId",
                table: "CountryNetworkDLStats",
                column: "CountryListId",
                principalTable: "CountryLists",
                principalColumn: "CountryListId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryDLStatOverviews_CountryLists_CountryListId",
                table: "CountryDLStatOverviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryNetworkDLStats_CountryLists_CountryListId",
                table: "CountryNetworkDLStats");

            migrationBuilder.DropIndex(
                name: "IX_CountryNetworkDLStats_CountryListId",
                table: "CountryNetworkDLStats");

            migrationBuilder.DropIndex(
                name: "IX_CountryDLStatOverviews_CountryListId",
                table: "CountryDLStatOverviews");

            migrationBuilder.DropColumn(
                name: "CountryListId",
                table: "CountryNetworkDLStats");

            migrationBuilder.DropColumn(
                name: "CountryListId",
                table: "CountryDLStatOverviews");
        }
    }
}
