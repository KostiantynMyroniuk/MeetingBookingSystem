using MediatR;
using MeetingBookingSystem.API.Common;
using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Models.MeetingRooms;

namespace MeetingBookingSystem.API.Features.MeetingRooms.CreateMeetingRoom
{
    public record CreateMeetingRoomCommand(
        string Name,
        string? Description) : IRequest<Result<MeetingRoomDto>>;

    public class CreateMeetingRoomCommandHandler(
        ApplicationDbContext context,
        ILogger<CreateMeetingRoomCommandHandler> logger) : IRequestHandler<CreateMeetingRoomCommand, Result<MeetingRoomDto>>
    {
        public async Task<Result<MeetingRoomDto>> Handle(CreateMeetingRoomCommand request, CancellationToken cancellationToken)
        {
            var meetingRoom = MeetingRoom.Create(request.Name, request.Description);

            context.MeetingRooms.Add(meetingRoom);
            await context.SaveChangesAsync(cancellationToken);

            return Result<MeetingRoomDto>.Success(
                new MeetingRoomDto(
                    meetingRoom.Id,
                    meetingRoom.Name,
                    meetingRoom.Description));
        }
    }
}
