namespace MeetingBookingSystem.API.Models.MeetingRooms;

public class MeetingRoom
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    private readonly List<MeetingRoomTimeSlot> _timeSlots = new();
    public IReadOnlyCollection<MeetingRoomTimeSlot> TimeSlots => _timeSlots.AsReadOnly();

    private MeetingRoom()
    {
    }

    public MeetingRoom(string name, string? description)
    {
        Id = Guid.CreateVersion7();
        Rename(name);
        UpdateDescription(description);
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Meeting room name is required.", nameof(name));
        }

        Name = name;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public MeetingRoomTimeSlot AddTimeSlot(DateTime startAt, DateTime endAt)
    {
        var timeSlot = new MeetingRoomTimeSlot(Id, startAt, endAt);
        _timeSlots.Add(timeSlot);
        return timeSlot;
    }

    public void RemoveTimeSlot(Guid timeSlotId)
    {
        var timeSlot = _timeSlots.FirstOrDefault(x => x.Id == timeSlotId)
            ?? throw new InvalidOperationException("Meeting room time slot was not found.");

        _timeSlots.Remove(timeSlot);
    }
}
