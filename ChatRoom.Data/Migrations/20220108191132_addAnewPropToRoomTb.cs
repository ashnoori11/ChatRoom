using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatRoom.Data.Migrations
{
    public partial class addAnewPropToRoomTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Rooms");
        }
    }
}
