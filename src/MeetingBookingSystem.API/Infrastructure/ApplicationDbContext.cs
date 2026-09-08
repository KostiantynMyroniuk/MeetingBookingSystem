using MeetingBookingSystem.API.Models.Bookings;
using MeetingBookingSystem.API.Models.Identity;
using MeetingBookingSystem.API.Models.MeetingRooms;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MeetingRoom> MeetingRooms { get; set; }

    public DbSet<MeetingRoomTimeSlot> MeetingRoomTimeSlots { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
