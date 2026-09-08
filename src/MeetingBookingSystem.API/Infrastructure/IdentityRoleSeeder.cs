using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace MeetingBookingSystem.API.Infrastructure;

public static class IdentityRoleSeeder
{
    private static readonly string[] SeedRoles = [IdentityRoles.User, IdentityRoles.Admin];

    public static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in SeedRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to seed role '{roleName}'.");
                }
            }
        }
    }
}
