using MeetingBookingSystem.API.Models.Bookings;

namespace MeetingBookingSystem.API.Models.MeetingRooms;

public class MeetingRoomTimeSlot
{
    public Guid Id { get; private set; }

    public Guid MeetingRoomId { get; private set; }
    public MeetingRoom MeetingRoom { get; private set; } = default!;

    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }

    public Booking? Booking { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    private MeetingRoomTimeSlot()
    {
    }

    public MeetingRoomTimeSlot(Guid meetingRoomId, DateTime startAt, DateTime endAt)
    {
        Id = Guid.CreateVersion7();
        MeetingRoomId = meetingRoomId;
        SetSchedule(startAt, endAt);
    }

    public void SetSchedule(DateTime startAt, DateTime endAt)
    {
        if (endAt <= startAt)
        {
            throw new ArgumentException("End time must be later than Start time.", nameof(endAt));
        }

        StartAt = startAt;
        EndAt = endAt;
    }

    public Booking Book(string bookedByUserId)
    {
        if (Booking is not null)
        {
            throw new InvalidOperationException("The time slot is already booked.");
        }

        var booking = new Booking(Id, bookedByUserId, DateTime.UtcNow);
        Booking = booking;

        return booking;
    }
}
