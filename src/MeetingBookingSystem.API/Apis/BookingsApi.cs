using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Extensions;
using MeetingBookingSystem.API.Features.Bookings;
using MeetingBookingSystem.API.Features.Bookings.CreateBooking;
using MeetingBookingSystem.API.Features.Bookings.GetMyBookings;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MeetingBookingSystem.API.Apis
{
    public static class BookingsApi
    {
        public static void MapBookingApi(this IEndpointRouteBuilder app)
        {
            var bookingGroup = app.MapGroup("api/bookings")
                .WithTags("Bookings");

            bookingGroup.MapPost("/", CreateBooking)
                .WithName("CreateBooking")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);

            bookingGroup.MapGet("my-bookings", GetMyBookings)
                .WithName("GetMyBookings")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);
        }

        public record CreateBookingRequest(Guid TimeSlotId);
        public static async Task<Results<Ok<BookingDto>, BadRequest, Conflict<string>, NotFound>> CreateBooking(
            [FromBody] CreateBookingRequest request,
            ISender sender,
            IHttpContextAccessor httpContextAccessor,
            CancellationToken ct)
        {
            var userId = httpContextAccessor.GetRequiredUserId();

            if (userId == null)
                return TypedResults.BadRequest();

            var result = await sender.Send(new CreateBookingCommand(userId, request.TimeSlotId), ct);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Value);

            return result.Error!.StatusCode switch
            {
                StatusCodes.Status404NotFound => TypedResults.NotFound(),
                StatusCodes.Status409Conflict => TypedResults.Conflict(result.Error.Message),
                _ => TypedResults.BadRequest()
            };
        }

        public static async Task<Results<Ok<PaginatedList<BookingDto>>, BadRequest>> GetMyBookings(
            ISender sender,
            IHttpContextAccessor httpContextAccessor,
            CancellationToken ct,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = httpContextAccessor.GetRequiredUserId();

            if (userId == null)
                return TypedResults.BadRequest();

            var result = await sender.Send(new GetMyBookingsQuery(userId, pageNumber, pageSize), ct);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Value);

            return TypedResults.BadRequest();
        }
    }
}
