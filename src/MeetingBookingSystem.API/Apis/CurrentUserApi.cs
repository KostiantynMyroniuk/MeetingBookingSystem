using System.Security.Claims;
using MeetingBookingSystem.API.Models.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MeetingBookingSystem.API.Apis
{
    public static class CurrentUserApi
    {
        public static void MapCurrentUserApi(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/me", GetCurrentUser)
                .WithName("GetCurrentUser")
                .WithTags("Me")
                .RequireAuthorization(AuthorizationPolicies.AnyUser);
        }

        public record CurrentUserDto(string? Email, IReadOnlyList<string> Roles);

        public static Ok<CurrentUserDto> GetCurrentUser(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            return TypedResults.Ok(new CurrentUserDto(email, roles));
        }
    }
}
