using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.Bookings.GetAllBookings
{
    public record GetAllBookingsQuery(
        int PageNumber,
        int PageSize) : IRequest<Result<PaginatedList<BookingDto>>>;

    public class GetAllBookings(
        ApplicationDbContext context) : IRequestHandler<GetAllBookingsQuery, Result<PaginatedList<BookingDto>>>
    {
        public async Task<Result<PaginatedList<BookingDto>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Bookings
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var bookings = await query
                .OrderByDescending(b => b.BookedAtUtc)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => new BookingDto(
                    b.Id,
                    b.MeetingRoomTimeSlot.MeetingRoomId,
                    b.MeetingRoomTimeSlot.MeetingRoom.Name,
                    b.MeetingRoomTimeSlotId,
                    b.MeetingRoomTimeSlot.StartAt,
                    b.MeetingRoomTimeSlot.EndAt,
                    b.BookedByUserId,
                    b.BookedAtUtc
                ))
                .ToListAsync(cancellationToken);

            return Result<PaginatedList<BookingDto>>.Success(
                new PaginatedList<BookingDto>(
                    bookings,
                    request.PageNumber,
                    request.PageSize,
                    totalCount));
        }
    }
}
