using MeetingBookingSystem.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        await app.Services.SeedRolesAsync();
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
