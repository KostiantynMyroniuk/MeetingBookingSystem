using MeetingBookingSystem.API.Models.Bookings;
using System.ComponentModel.DataAnnotations;

namespace MeetingBookingSystem.API.Models.MeetingRooms;

public class MeetingRoomTimeSlot
{
    public Guid Id { get; set; }

    public Guid MeetingRoomId { get; set; }

    public MeetingRoom MeetingRoom { get; set; } = null!;

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public Booking? Booking { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
