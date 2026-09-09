namespace MeetingBookingSystem.API.Features.MeetingRoomTimeSlots
{
    public record TimeSlotDto(
        Guid Id,
        Guid MeetingRoomId,
        DateTime StartAt,
        DateTime EndAt,
        bool IsBooked);

}
