using MeetingBookingSystem.API.Models.Bookings;
using Microsoft.AspNetCore.Identity;

namespace MeetingBookingSystem.API.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
