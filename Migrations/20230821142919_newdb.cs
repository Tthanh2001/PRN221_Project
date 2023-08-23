using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Project.Migrations
{
    public partial class newdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfSeats",
                table: "Rooms",
                newName: "NumberOfRows");

            migrationBuilder.AddColumn<string>(
                name: "SeatName",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCols",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatName",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "NumberOfCols",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "NumberOfRows",
                table: "Rooms",
                newName: "NumberOfSeats");
        }
    }
}
