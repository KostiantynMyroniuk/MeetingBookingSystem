using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Features.MeetingRooms;
using MeetingBookingSystem.API.Features.MeetingRooms.CreateMeetingRoom;
using MeetingBookingSystem.API.Features.MeetingRooms.DeleteMeetingRoom;
using MeetingBookingSystem.API.Features.MeetingRooms.GetMeetingRoomById;
using MeetingBookingSystem.API.Features.MeetingRooms.GetMeetingRooms;
using MeetingBookingSystem.API.Features.MeetingRooms.UpdateMeetingRoom;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MeetingBookingSystem.API.Apis
{
    public static class MeetingRoomsApi
    {
        public static void MapMeetingRoomsApi(this IEndpointRouteBuilder app)
        {
            var meetingRoomsGroup = app.MapGroup("/api/rooms")
                .WithTags("MeetingRooms");

            meetingRoomsGroup.MapPost("/", CreateMeetingRoom)
                .WithName("CreateMeetingRoom")
                .RequireAuthorization(AuthorizationPolicies.AdminOnly);

            meetingRoomsGroup.MapGet("/{meetingRoomId:guid}", GetMeetingRoomById)
                .WithName("GetMeetingRoomById")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);

            meetingRoomsGroup.MapGet("/", GetMeetingRooms)
                .WithName("GetMeetingRooms")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);

            meetingRoomsGroup.MapPut("/{meetingRoomId:guid}", UpdateMeetingRoom)
                .WithName("UpdateMeetingRoom")
                .RequireAuthorization(AuthorizationPolicies.AdminOnly); ;

            meetingRoomsGroup.MapDelete("/{meetingRoomId:guid}", DeleteMeetingRoom)
                .WithName("DeleteMeetingRoom")
                .RequireAuthorization(AuthorizationPolicies.AdminOnly);
        }

        public record CreateMeetingRoomRequest(string Name, string? Description);
        public static async Task<Created<MeetingRoomDto>> CreateMeetingRoom(
            CreateMeetingRoomRequest request,
            ISender sender,
            CancellationToken ct)
        {
            var result = await sender.Send(new CreateMeetingRoomCommand(request.Name, request.Description), ct);

            return TypedResults.Created($"/api/rooms/{result.Value!.Id}", result.Value);
        }

        public static async Task<Results<Ok<MeetingRoomDto>, NotFound>> GetMeetingRoomById(
            [FromRoute] Guid meetingRoomDto,
            ISender sender,
            CancellationToken ct)
        {
            var result = await sender.Send(new GetMeetingRoomByIdQuery(meetingRoomDto), ct);

            return result.IsSuccess 
                ? TypedResults.Ok(result.Value) 
                : TypedResults.NotFound();
        }

        public static async Task<Ok<PaginatedList<MeetingRoomDto>>> GetMeetingRooms(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            ISender sender,
            CancellationToken ct)
        {
            var result = await sender.Send(new GetMeetingRoomsQuery(pageNumber, pageSize), ct);

            return TypedResults.Ok(result.Value);
        }

        public record UpdateMeetingRoomRequest(string Name, string? Description);
        public static async Task<Results<Ok<MeetingRoomDto>, NotFound>> UpdateMeetingRoom(
            [FromRoute] Guid meetingRoomId,
            UpdateMeetingRoomRequest request,
            ISender sender,
            CancellationToken ct)
        {
            var result = await sender.Send(new UpdateMeetingRoomCommand(
                meetingRoomId,
                request.Name,
                request.Description), ct);

            return result.IsSuccess 
                ? TypedResults.Ok(result.Value) 
                : TypedResults.NotFound();
        }

        public static async Task<Results<NoContent, NotFound>> DeleteMeetingRoom(
            [FromRoute] Guid meetingRoomId,
            ISender sender,
            CancellationToken ct) 
        {
            var result = await sender.Send(new DeleteMeetingRoomCommand(meetingRoomId), ct);

            return result.IsSuccess 
                ? TypedResults.NoContent() 
                : TypedResults.NotFound();
        }
    }
}
