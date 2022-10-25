using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretBot.DataAccess.Migrations
{
    public partial class Puuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LolPuuid",
                table: "LolUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LolPuuid",
                table: "LolUsers");
        }
    }
}
