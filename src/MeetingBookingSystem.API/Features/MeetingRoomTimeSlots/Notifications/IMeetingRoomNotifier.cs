namespace MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications
{
    public interface IMeetingRoomNotifier
    {
        Task NotifySlotBooked(Guid meetingRoomId, TimeSlotDto timeSlotDto);
    }
}
