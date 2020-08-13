using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class UpdateRegionDLStatDetailColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "RegionDLStatDetails",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "RegionDLStatDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Full_DateTime",
                table: "RegionDLStatDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "RegionDLStatDetails");

            migrationBuilder.DropColumn(
                name: "Full_DateTime",
                table: "RegionDLStatDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "RegionDLStatDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
