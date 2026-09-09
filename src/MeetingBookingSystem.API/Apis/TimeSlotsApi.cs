using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Features.MeetingRooms.GetMeetingRooms;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.GetTimeSlots;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MeetingBookingSystem.API.Apis
{
    public static class TimeSlotsApi
    {
        public static void MapTimeSlotsApi(this IEndpointRouteBuilder app)
        {
            var timeSlotsGroup = app.MapGroup("/api/rooms/{meetingRoomId:guid}/slots")
                .WithTags("TimeSlots");

            timeSlotsGroup.MapGet("/", GetTimeSlots)
                .WithName("GetTimeSlots")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);
        }

        public static async Task<Ok<PaginatedList<TimeSlotDto>>> GetTimeSlots(
            Guid meetingRoomId,
            ISender sender,
            CancellationToken ct,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await sender.Send(new GetTimeSlotsQuery(meetingRoomId, pageNumber, pageSize), ct);

            return TypedResults.Ok(result.Value);
        }
    }
}