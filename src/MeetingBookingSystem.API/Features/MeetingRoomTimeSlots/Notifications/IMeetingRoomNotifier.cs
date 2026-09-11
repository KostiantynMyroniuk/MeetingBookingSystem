namespace MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications
{
    public interface IMeetingRoomNotifier
    {
        Task NotifySlotStatusChanged(Guid meetingRoomId, TimeSlotDto timeSlotDto);
    }
}
