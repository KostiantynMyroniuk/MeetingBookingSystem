using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Middleware;
using MeetingBookingSystem.API.Models.Identity;
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

        builder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
        });

        builder.Services.AddOpenApi();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ApiExceptionHandler>();
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

    private static void RegisterHandlers(IHostApplicationBuilder builder)
    {
        var assembly = typeof(Extensions).Assembly;
        var handlerTypes = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.Name.EndsWith("Handler"));

        foreach (var handlerType in handlerTypes)
        {
            var serviceType = handlerType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.Name == $"I{handlerType.Name}");
            if (serviceType is null)
            {
                continue;
            }

            builder.Services.AddScoped(serviceType, handlerType);
        }
    }
}
