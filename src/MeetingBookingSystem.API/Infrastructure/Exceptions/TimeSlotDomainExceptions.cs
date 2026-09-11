namespace MeetingBookingSystem.API.Infrastructure.Exceptions
{
    public class TimeSlotValidationException(string message) : DomainException(message);
    public class TimeSlotConflictException(string message) : DomainException(message);
}
