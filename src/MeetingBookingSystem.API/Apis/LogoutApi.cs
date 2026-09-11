using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace MeetingBookingSystem.API.Apis
{
    public static class LogoutApi
    {
        public static void MapLogoutApi<TUser>(this IEndpointRouteBuilder app) where TUser : class
        {
            app.MapPost("/logout", async (SignInManager<TUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok(new { message = "Logged out successfully." });
            })
            .RequireAuthorization();
        }
    }
}
