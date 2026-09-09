using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Features.MeetingRooms.DeleteMeetingRoom
{
    public record DeleteMeetingRoomCommand(
        Guid MeetingRoomId) : IRequest<Result>;

    public class DeleteMeetingRoomCommandHandler(
        ApplicationDbContext context,
        ILogger<DeleteMeetingRoomCommandHandler> logger) : IRequestHandler<DeleteMeetingRoomCommand, Result>
    {
        public async Task<Result> Handle(DeleteMeetingRoomCommand request, CancellationToken cancellationToken)
        {
            var deletedRows = await context.MeetingRooms
                .Where(r => r.Id == request.MeetingRoomId)
                .ExecuteDeleteAsync();

            if (deletedRows == 0)
            {
                logger.LogWarning("Meeting room {MeetingRoomId} not found.", request.MeetingRoomId);
                return Result.Failure(ResultError.NotFound("Meeting room not found."));
            }

            return Result.Success();
        }
    }
}
