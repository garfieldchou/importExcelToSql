using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class RemoveUnusedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "ProceUsageDetails");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ProceUsageDetails");

            migrationBuilder.DropColumn(
                name: "WorkWeek",
                table: "ProceUsageDetails");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "PCVideoCardUsageDetails");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "PCVideoCardUsageDetails");

            migrationBuilder.DropColumn(
                name: "WorkWeek",
                table: "PCVideoCardUsageDetails");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "PcPhyCpuDetails");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "PcPhyCpuDetails");

            migrationBuilder.DropColumn(
                name: "WorkWeek",
                table: "PcPhyCpuDetails");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "DirectXOSs");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "DirectXOSs");

            migrationBuilder.DropColumn(
                name: "WorkWeek",
                table: "DirectXOSs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "ProceUsageDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ProceUsageDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkWeek",
                table: "ProceUsageDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "PCVideoCardUsageDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "PCVideoCardUsageDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkWeek",
                table: "PCVideoCardUsageDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "PcPhyCpuDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "PcPhyCpuDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkWeek",
                table: "PcPhyCpuDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "DirectXOSs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "DirectXOSs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkWeek",
                table: "DirectXOSs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
