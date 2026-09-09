using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.MeetingRooms.UpdateMeetingRoom
{
    public record UpdateMeetingRoomCommand(
        Guid MeetingRoomId,
        string Name,
        string? Description) : IRequest<Result<MeetingRoomDto>>;

    public class UpdateMeetingRoomCommandHandler(
        ApplicationDbContext context,
        ILogger<UpdateMeetingRoomCommandHandler> logger) : IRequestHandler<UpdateMeetingRoomCommand, Result<MeetingRoomDto>>
    {
        public async Task<Result<MeetingRoomDto>> Handle(UpdateMeetingRoomCommand request, CancellationToken cancellationToken)
        {
            var meetingRoom = await context.MeetingRooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.MeetingRoomId, cancellationToken);

            if (meetingRoom == null)
            {
                logger.LogWarning("Meeting room {MeetingRoomId} not found", request.MeetingRoomId);
                return Result<MeetingRoomDto>.Failure(ResultError.NotFound("Meeting room not found."));
            }

            meetingRoom.Rename(request.Name);
            meetingRoom.UpdateDescription(request.Description);

            await context.SaveChangesAsync(cancellationToken);

            return Result<MeetingRoomDto>.Success(new MeetingRoomDto(
                meetingRoom.Id,
                meetingRoom.Name,
                meetingRoom.Description));
        }
    }
}
