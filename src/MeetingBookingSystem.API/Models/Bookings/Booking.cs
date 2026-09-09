using MeetingBookingSystem.API.Models.Identity;
using MeetingBookingSystem.API.Models.MeetingRooms;

namespace MeetingBookingSystem.API.Models.Bookings;

public class Booking
{
    public Guid Id { get; private set; }

    public Guid MeetingRoomTimeSlotId { get; private set; }
    public MeetingRoomTimeSlot MeetingRoomTimeSlot { get; private set; } = default!;

    public string BookedByUserId { get; private set; } = string.Empty;
    public ApplicationUser BookedByUser { get; private set; } = default!;

    public DateTime BookedAtUtc { get; private set; }

    private Booking()
    {
    }

    public Booking(Guid timeSlotId, string bookedByUserId, DateTime bookedAtUtc)
    {
        Id = Guid.CreateVersion7();
        MeetingRoomTimeSlotId = timeSlotId;
        BookedByUserId = bookedByUserId;
        BookedAtUtc = bookedAtUtc;
    }
}
