using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone_API.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Short_Name",
                table: "Building",
                newName: "ShortName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Building",
                newName: "Short_Name");
        }
    }
}
