using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MeetingBookingSystem.API.Apis.Hubs
{
    [Authorize(Policy = AuthorizationPolicies.AnyUser)]
    public class MeetingRoomHub : Hub<IMeetingRoomClient>
    {
        public async Task JoinGroup(Guid meetingRoomId) =>
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(meetingRoomId));

        public async Task LeaveGroup(Guid meetingRoomId) =>
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(meetingRoomId));

        public static string GetGroupName(Guid meetingRoomId) => $"room-{meetingRoomId}";
    }
}
