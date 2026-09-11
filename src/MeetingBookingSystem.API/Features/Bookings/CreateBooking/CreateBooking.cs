using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots;
using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications;
using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Models.Bookings;
using MeetingBookingSystem.API.Models.MeetingRooms;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.Bookings.CreateBooking
{
    public record CreateBookingCommand(
        string UserId,
        Guid TimeSlotId) : IRequest<Result<BookingDto>>;

    public class CreateBookingCommandHandler(
        ApplicationDbContext context,
        IMeetingRoomNotifier roomNotifier,
        ILogger<CreateBookingCommandHandler> logger) : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
    {
        public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var timeSlot = await context.MeetingRoomTimeSlots
                .Include(s => s.MeetingRoom)
                .FirstOrDefaultAsync(s => s.Id == request.TimeSlotId, cancellationToken);

            if (timeSlot == null)
            {
                logger.LogWarning("Meeting time slot {MeetingTimeSlot} was not found by User {UserId}.", request.TimeSlotId, request.UserId);
                return Result<BookingDto>.Failure(ResultError.NotFound("Meeting time slot not found."));
            }

            var booking = timeSlot.Book(request.UserId);

            try
            {
                context.Bookings.Add(booking);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogWarning(ex, "Time slot {MeetingTimeSlot} already booked", request.TimeSlotId);

                return Result<BookingDto>.Failure(ResultError.Conflict("Time slot already booked"));
            }
            catch (DbUpdateException ex)
            {
                logger.LogWarning(ex, "Time slot {MeetingTimeSlot} already booked", request.TimeSlotId);

                return Result<BookingDto>.Failure(ResultError.Conflict("Time slot already booked"));
            }

            /// Notifying

            try
            {
                var slotDto = new TimeSlotDto(
                    timeSlot.Id,
                    timeSlot.MeetingRoomId,
                    timeSlot.StartAt,
                    timeSlot.EndAt,
                    IsBooked: true);

                await roomNotifier.NotifySlotStatusChanged(timeSlot.MeetingRoomId, slotDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to notify clients about booked slot {MeetingTimeSlot}", request.TimeSlotId);
            }

            return Result<BookingDto>.Success(new BookingDto(
                booking.Id, 
                timeSlot.MeetingRoomId,
                timeSlot.MeetingRoom.Name, 
                booking.MeetingRoomTimeSlotId,
                timeSlot.StartAt,
                timeSlot.EndAt,
                booking.BookedByUserId,
                booking.BookedAtUtc));
        }
    }
}
