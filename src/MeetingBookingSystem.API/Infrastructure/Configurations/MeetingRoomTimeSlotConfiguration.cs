using MeetingBookingSystem.API.Models.Bookings;
using MeetingBookingSystem.API.Models.MeetingRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingBookingSystem.API.Infrastructure.Configurations;

public class MeetingRoomTimeSlotConfiguration : IEntityTypeConfiguration<MeetingRoomTimeSlot>
{
    public void Configure(EntityTypeBuilder<MeetingRoomTimeSlot> builder)
    {
        builder.HasOne(x => x.MeetingRoom)
            .WithMany(x => x.TimeSlots)
            .HasForeignKey(x => x.MeetingRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Booking)
            .WithOne(x => x.MeetingRoomTimeSlot)
            .HasForeignKey<Booking>(x => x.MeetingRoomTimeSlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
