using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MeetingBookingSystem.API.Infrastructure.Seeders
{
    public class AdminOptions
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public static class AdminSeeder
    {
        public static async Task SeedSuperAdminAsync(this IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminOptions = serviceProvider.GetRequiredService<IOptions<AdminOptions>>().Value;

            var email = adminOptions.Email;
            var password = adminOptions.Password;

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create admin: {string.Join(", ", result.Errors.Select(x => x.Description))}");
                }
            }
            
            if (!await userManager.IsInRoleAsync(user, IdentityRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, IdentityRoles.Admin);
            }
        }
    }
}
