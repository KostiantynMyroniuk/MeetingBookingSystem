using MeetingBookingSystem.API.Models.MeetingRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingBookingSystem.API.Infrastructure.Configurations;

public class MeetingRoomConfiguration : IEntityTypeConfiguration<MeetingRoom>
{
    public void Configure(EntityTypeBuilder<MeetingRoom> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Location).HasMaxLength(200);
    }
}
