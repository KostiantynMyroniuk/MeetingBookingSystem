using MeetingBookingSystem.API.Models.Identity;
using MeetingBookingSystem.API.Models.MeetingRooms;

namespace MeetingBookingSystem.API.Models.Bookings;

public class Booking
{
    public Guid Id { get; set; }

    public Guid MeetingRoomTimeSlotId { get; set; }

    public MeetingRoomTimeSlot MeetingRoomTimeSlot { get; set; } = null!;

    public string BookedByUserId { get; set; } = string.Empty;

    public ApplicationUser BookedByUser { get; set; } = null!;

    public DateTime BookedAtUtc { get; set; }
}
