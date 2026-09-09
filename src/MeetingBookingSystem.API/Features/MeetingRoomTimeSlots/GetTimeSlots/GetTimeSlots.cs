using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Features.MeetingRooms;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.GetTimeSlots
{
    public record GetTimeSlotsQuery(
        Guid MeetingRoomId,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<Result<PaginatedList<TimeSlotDto>>>;

    public class GetTimeSlotsQueryHandler(
        ApplicationDbContext context) : IRequestHandler<GetTimeSlotsQuery, Result<PaginatedList<TimeSlotDto>>>
    {
        public async Task<Result<PaginatedList<TimeSlotDto>>> Handle(GetTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var query = context.MeetingRoomTimeSlots.AsNoTracking();
            var totalCount = await query
                .Where(s => s.MeetingRoomId == request.MeetingRoomId)
                .CountAsync(cancellationToken);

            var slots = await query
                .Where(s => s.MeetingRoomId == request.MeetingRoomId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new TimeSlotDto(
                    s.Id,
                    s.MeetingRoomId,
                    s.StartAt,
                    s.EndAt,
                    s.IsBooked))
                .ToListAsync(cancellationToken);

            return Result<PaginatedList<TimeSlotDto>>.Success(new PaginatedList<TimeSlotDto>(slots, request.PageNumber, request.PageSize, totalCount));
        }
    }
}
