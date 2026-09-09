namespace MeetingBookingSystem.API.Models.Identity;

public static class AuthorizationPolicies
{
    public const string AnyUser = nameof(AnyUser);

    public const string AdminOnly = nameof(AdminOnly);
}
