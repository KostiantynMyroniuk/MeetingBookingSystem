using System.Net;
using System.Net.Http.Json;
using MeetingBookingSystem.API.Infrastructure;
using MeetingBookingSystem.API.Models.MeetingRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MeetingBookingSystem.Tests;

public class BookingConcurrencyTests : IClassFixture<BookingConcurrencyTestFactory>
{
    private const int ConcurrentRequests = 20;

    private readonly BookingConcurrencyTestFactory _factory;

    public BookingConcurrencyTests(BookingConcurrencyTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ConcurrentBookingRequestsForSameSlot_OnlyOneSucceeds()
    {
        var timeSlotId = await SeedTimeSlotAsync();

        var client = _factory.CreateClient();
        var authCookie = await RegisterAndLoginAsync(client);

        var bookingResponses = await Task.WhenAll(
            Enumerable.Range(0, ConcurrentRequests)
                .Select(_ => PostBookingAsync(client, authCookie, timeSlotId)));

        Assert.All(bookingResponses, response =>
            Assert.True(
                response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Conflict,
                $"Unexpected status code {response.StatusCode}, booking must either succeed or report a conflict, never a server error."));

        var successCount = bookingResponses.Count(r => r.StatusCode == HttpStatusCode.OK);
        var conflictCount = bookingResponses.Count(r => r.StatusCode == HttpStatusCode.Conflict);

        Assert.Equal(1, successCount);
        Assert.Equal(ConcurrentRequests - 1, conflictCount);

        await AssertExactlyOneBookingPersistedAsync(timeSlotId);
    }

    private async Task<Guid> SeedTimeSlotAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var room = new MeetingRoom("Concurrency Test Room", null);
        var timeSlot = new MeetingRoomTimeSlot(room.Id, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1));

        db.MeetingRooms.Add(room);
        db.MeetingRoomTimeSlots.Add(timeSlot);
        await db.SaveChangesAsync();

        return timeSlot.Id;
    }

    private async Task AssertExactlyOneBookingPersistedAsync(Guid timeSlotId)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var bookingsForSlot = await db.Bookings.CountAsync(b => b.MeetingRoomTimeSlotId == timeSlotId);

        Assert.Equal(1, bookingsForSlot);
    }

    private static async Task<string> RegisterAndLoginAsync(HttpClient client)
    {
        var email = $"concurrency-{Guid.NewGuid():N}@test.com";
        const string password = "TestPassword123!";

        var registerResponse = await client.PostAsJsonAsync("/api/register", new { email, password });
        registerResponse.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/login?useCookies=true", new { email, password });
        loginResponse.EnsureSuccessStatusCode();

        var authCookie = loginResponse.Headers.GetValues("Set-Cookie")
            .First(c => c.StartsWith(".AspNetCore.Identity.Application", StringComparison.Ordinal));

        return authCookie.Split(';')[0];
    }

    private static Task<HttpResponseMessage> PostBookingAsync(HttpClient client, string authCookie, Guid timeSlotId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/bookings")
        {
            Content = JsonContent.Create(new { timeSlotId })
        };
        request.Headers.Add("Cookie", authCookie);

        return client.SendAsync(request);
    }
}
