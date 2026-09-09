using MediatR;
using MeetingBookingSystem.API.Common;
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
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogWarning(ex, "Time slot {MeetingTimeSlot} already booked", request.TimeSlotId);

                return Result<BookingDto>.Failure(ResultError.Conflict("Time slot already booked"));
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
