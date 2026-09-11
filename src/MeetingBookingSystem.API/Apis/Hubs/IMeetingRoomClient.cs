using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots;

namespace MeetingBookingSystem.API.Apis.Hubs
{
    public interface IMeetingRoomClient
    {
        Task SlotStatusChanged(TimeSlotDto timeSlotDto);
    }
}
