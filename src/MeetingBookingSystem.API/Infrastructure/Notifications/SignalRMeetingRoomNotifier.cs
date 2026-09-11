using MeetingBookingSystem.API.Apis.Hubs;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace MeetingBookingSystem.API.Infrastructure.Notifications
{
    public class SignalRMeetingRoomNotifier(
        IHubContext<MeetingRoomHub, IMeetingRoomClient> hubContext) : IMeetingRoomNotifier
    {
        public Task NotifySlotStatusChanged(Guid meetingRoomId, TimeSlotDto timeSlotDto)
        {
            return hubContext
                .Clients
                .Group(MeetingRoomHub.GetGroupName(meetingRoomId))
                .SlotStatusChanged(timeSlotDto);
        }
    }
}
