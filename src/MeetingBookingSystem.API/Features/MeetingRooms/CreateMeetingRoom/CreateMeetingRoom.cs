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
        private const int DefaultSlotDays = 7;
        private const int DefaultSlotStartHour = 9;
        private const int DefaultSlotEndHour = 18;

        public async Task<Result<MeetingRoomDto>> Handle(CreateMeetingRoomCommand request, CancellationToken cancellationToken)
        {
            var meetingRoom = MeetingRoom.Create(request.Name, request.Description);

            context.MeetingRooms.Add(meetingRoom);
            context.MeetingRoomTimeSlots.AddRange(GenerateDefaultTimeSlots(meetingRoom.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Result<MeetingRoomDto>.Success(
                new MeetingRoomDto(
                    meetingRoom.Id,
                    meetingRoom.Name,
                    meetingRoom.Description));
        }

        private static IEnumerable<MeetingRoomTimeSlot> GenerateDefaultTimeSlots(Guid meetingRoomId)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(1);

            for (var day = 0; day < DefaultSlotDays; day++)
            {
                var date = startDate.AddDays(day);

                for (var hour = DefaultSlotStartHour; hour < DefaultSlotEndHour; hour++)
                {
                    var startAt = date.AddHours(hour);
                    var endAt = startAt.AddHours(1);

                    yield return new MeetingRoomTimeSlot(meetingRoomId, startAt, endAt);
                }
            }
        }
    }
}
