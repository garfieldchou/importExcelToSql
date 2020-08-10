using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class MakeCountryNameUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "GameRanks",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CountryLists",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CountryDLStatOverviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryLists_Country",
                table: "CountryLists",
                column: "Country",
                unique: true,
                filter: "[Country] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CountryLists_Country",
                table: "CountryLists");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CountryDLStatOverviews");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "GameRanks",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CountryLists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
