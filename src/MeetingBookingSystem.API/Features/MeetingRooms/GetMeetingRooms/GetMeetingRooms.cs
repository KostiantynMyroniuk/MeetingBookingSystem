using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.MeetingRooms.GetMeetingRooms
{
    public record GetMeetingRoomsQuery(
        int PageNumber = 1,
        int PageSize = 10) : IRequest<Result<PaginatedList<MeetingRoomDto>>>;

    public class GetMeetingRoomsQueryHandler(
        ApplicationDbContext context) : IRequestHandler<GetMeetingRoomsQuery, Result<PaginatedList<MeetingRoomDto>>>
    {
        public async Task<Result<PaginatedList<MeetingRoomDto>>> Handle(GetMeetingRoomsQuery request, CancellationToken cancellationToken)
        {
            var query = context.MeetingRooms.AsNoTracking();
            var totalCount = await query.CountAsync(cancellationToken);

            var rooms = await query
                .OrderBy(r => r.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(room => new MeetingRoomDto(
                    room.Id,
                    room.Name,
                    room.Description))
                .ToListAsync(cancellationToken);

            return Result<PaginatedList<MeetingRoomDto>>.Success(new PaginatedList<MeetingRoomDto>(rooms, request.PageNumber, request.PageSize, totalCount));
        }
    }
}
