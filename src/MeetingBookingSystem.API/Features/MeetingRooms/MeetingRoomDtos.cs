namespace MeetingBookingSystem.API.Features.MeetingRooms;

public sealed record MeetingRoomDto(Guid Id, string Name, string? Description);

public sealed record MeetingRoomDetailsDto(Guid Id, string Name, string? Description, IReadOnlyCollection<MeetingRoomTimeSlotDto> TimeSlots);

public sealed record MeetingRoomTimeSlotDto(Guid Id, DateTimeOffset StartAt, DateTimeOffset EndAt, bool IsBooked);
