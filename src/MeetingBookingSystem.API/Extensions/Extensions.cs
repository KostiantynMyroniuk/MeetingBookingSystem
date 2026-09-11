using MeetingBookingSystem.API.Features.MeetingRoomTimeSlots.Notifications;
using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Infrastructure.Notifications;
using MeetingBookingSystem.API.Infrastructure.Seeders;
using MeetingBookingSystem.API.Middleware;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.SignalR;
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

        builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection("Admin"));

        builder.Services.AddEndpointsApiExplorer();
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
        var signalRBuilder = builder.Services.AddSignalR();

        var azureSignalRConnectionString = builder.Configuration.GetConnectionString("AzureSignalR");

        if (!string.IsNullOrWhiteSpace(azureSignalRConnectionString))
        {
            signalRBuilder.AddAzureSignalR(azureSignalRConnectionString);
        }

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
            .AddIdentityApiEndpoints<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy(AuthorizationPolicies.AnyUser, policy =>
                policy.RequireAuthenticatedUser());

            option.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
                policy.RequireRole(IdentityRoles.Admin));
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(1);
            options.SlidingExpiration = true;
        });

        builder.Services.AddHttpContextAccessor();
    }
}
