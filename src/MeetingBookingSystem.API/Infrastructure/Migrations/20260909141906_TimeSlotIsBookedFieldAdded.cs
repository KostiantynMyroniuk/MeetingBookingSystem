using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingBookingSystem.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimeSlotIsBookedFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "MeetingRoomTimeSlots",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "MeetingRoomTimeSlots");
        }
    }
}
