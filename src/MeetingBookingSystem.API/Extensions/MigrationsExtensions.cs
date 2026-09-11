using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace MeetingBookingSystem.API.Extensions;

public static class MigrationsExtensions
{
    public static async Task SeedDatabaseDefaults(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        await serviceProvider.SeedRolesAsync();
        await serviceProvider.SeedSuperAdminAsync();
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
