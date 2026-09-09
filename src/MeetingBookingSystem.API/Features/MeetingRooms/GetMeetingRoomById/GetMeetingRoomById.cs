using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.MeetingRooms.GetMeetingRoomById
{
    public record GetMeetingRoomByIdQuery(
        Guid MeetingRoomId) : IRequest<Result<MeetingRoomDto>>;

    public class GetMeetingRoomByIdQueryHandler(
        ApplicationDbContext context,
        ILogger<GetMeetingRoomByIdQueryHandler> logger) : IRequestHandler<GetMeetingRoomByIdQuery, Result<MeetingRoomDto>>
    {
        public async Task<Result<MeetingRoomDto>> Handle(GetMeetingRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var meetingRoom = await context.MeetingRooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.MeetingRoomId, cancellationToken);

            if (meetingRoom == null)
            {
                logger.LogWarning("Meeting room {MeetingRoomId} not found", request.MeetingRoomId);
                return Result<MeetingRoomDto>.Failure(ResultError.NotFound("Meeting room not found."));
            }

            return Result<MeetingRoomDto>.Success(new MeetingRoomDto(
                meetingRoom.Id,
                meetingRoom.Name,
                meetingRoom.Description));
        }
    }
}
