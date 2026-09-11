namespace MeetingBookingSystem.API.Infrastructure.Exceptions
{
    public class MeetingRoomValidationException(string message) : DomainException(message)
    { 
    }
}
