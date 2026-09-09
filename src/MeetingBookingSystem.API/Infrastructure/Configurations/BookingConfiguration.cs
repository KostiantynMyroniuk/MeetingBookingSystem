using MeetingBookingSystem.API.Models.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingBookingSystem.API.Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(x => x.BookedByUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.BookedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.BookedByUser)
            .WithMany()
            .HasForeignKey(x => x.BookedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
