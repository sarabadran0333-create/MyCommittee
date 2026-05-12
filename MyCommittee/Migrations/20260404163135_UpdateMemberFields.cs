using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCommittee.Migrations
{
    public partial class UpdateMemberFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Members",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Members",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");
        }
    }
}