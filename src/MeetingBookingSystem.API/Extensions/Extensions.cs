using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BookingServiceDb")
                                ?? throw new InvalidOperationException("Connection string 'BookingServiceDb' is not configured.")));
    }

    public static void AddIdentityServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddHttpContextAccessor();
    }
}
