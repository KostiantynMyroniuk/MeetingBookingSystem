namespace MeetingBookingSystem.API.Features.Bookings;

public sealed record BookingDto(
    Guid Id, 
    Guid MeetingRoomId,
    string MeetingRoomName,
    Guid MeetingRoomTimeSlotId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string BookedByUserId,
    DateTimeOffset BookedAtUtc);
