using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Project.Migrations
{
    public partial class changeseat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatPosition",
                table: "Seats");

            migrationBuilder.AddColumn<int>(
                name: "SeatCol",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeatRow",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatCol",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatRow",
                table: "Seats");

            migrationBuilder.AddColumn<string>(
                name: "SeatPosition",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
