using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class ChangeAvgAndMaxToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Max",
                table: "RegionDLStatOverviews",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Average",
                table: "RegionDLStatOverviews",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Max",
                table: "RegionDLStatOverviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "Average",
                table: "RegionDLStatOverviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
