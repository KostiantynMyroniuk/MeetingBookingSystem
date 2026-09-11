using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications;
using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Infrastructure.Notifications;
using MeetingBookingSystem.API.Middleware;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
        });

        builder.Services.AddSwaggerGen();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("BookingServiceDb") 
                    ?? throw new InvalidOperationException("Connection string 'BookingServiceDb' is not configured."),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    );
                }));
    }

    public static void AddSignalR(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSignalR();

        builder.Services.AddScoped<IMeetingRoomNotifier, SignalRMeetingRoomNotifier>();
    }

    public static void AddCors(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("BookingFrontend", policy =>
            {
                policy
                    .WithOrigins(builder.Configuration["Frontend:BaseUrl"] ?? throw new InvalidOperationException("Frontend 'BaseUrl' not configured"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static void AddIdentityServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy(AuthorizationPolicies.AnyUser, policy =>
                policy.RequireAuthenticatedUser());

            option.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
                policy.RequireRole(IdentityRoles.Admin));
        });

        builder.Services.AddHttpContextAccessor();
    }
}
