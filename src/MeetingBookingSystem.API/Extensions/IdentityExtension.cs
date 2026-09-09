using System.Security.Claims;

namespace MeetingBookingSystem.API.Extensions;

public static class IdentityExtension
{
    public static string GetRequiredUserId(this IHttpContextAccessor accessor)
    {
        return accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    public static bool IsInRole(this IHttpContextAccessor accessor, string role)
        => accessor.HttpContext?.User.IsInRole(role) == true;
}
