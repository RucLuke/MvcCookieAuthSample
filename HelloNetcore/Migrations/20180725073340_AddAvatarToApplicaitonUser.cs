using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloNetcore.Migrations
{
    public partial class AddAvatarToApplicaitonUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avator",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avator",
                table: "AspNetUsers");
        }
    }
}
