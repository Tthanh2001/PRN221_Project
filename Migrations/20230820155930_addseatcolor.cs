using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Project.Migrations
{
    public partial class addseatcolor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatColor",
                table: "SeatTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatColor",
                table: "SeatTypes");
        }
    }
}
