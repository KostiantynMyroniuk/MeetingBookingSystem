using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.Bookings.CancelBooking
{
    public record CancelBookingCommand(
        string UserId,
        Guid BookingId) : IRequest<Result>;

    public class CancelBookingCommandHandler(
        ApplicationDbContext context,
        IMeetingRoomNotifier roomNotifier,
        ILogger<CancelBookingCommandHandler> logger) : IRequestHandler<CancelBookingCommand, Result>
    {
        public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await context.Bookings
                .Include(b => b.MeetingRoomTimeSlot)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return Result.Failure(ResultError.NotFound("Booking not found."));
            }

            if (booking.BookedByUserId != request.UserId)
            {
                return Result.Failure(ResultError.Forbidden("You can only cancel your own booking."));
            }

            var timeSlot = booking.MeetingRoomTimeSlot;
            timeSlot.Release();

            context.Bookings.Remove(booking);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                logger.LogWarning(ex, "Booking {BookingId} could not be cancelled due to a concurrent change", request.BookingId);

                return Result.Failure(ResultError.Conflict("The booking could not be cancelled, please retry."));
            }

            try
            {
                var slotDto = new TimeSlotDto(
                    timeSlot.Id,
                    timeSlot.MeetingRoomId,
                    timeSlot.StartAt,
                    timeSlot.EndAt,
                    IsBooked: false);

                await roomNotifier.NotifySlotStatusChanged(timeSlot.MeetingRoomId, slotDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to notify clients about released slot {MeetingTimeSlot}", timeSlot.Id);
            }

            return Result.Success();
        }
    }
}
