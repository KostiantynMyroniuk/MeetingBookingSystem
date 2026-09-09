using MeetingBookingSystem.API.Apis;
using MeetingBookingSystem.API.Extensions;
using MeetingBookingSystem.API.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddIdentityServices();
builder.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedRolesAsync();

app.MapIdentityApi<ApplicationUser>();
app.MapMeetingRoomsApi();
app.MapTimeSlotsApi();
app.MapBookingApi();

app.Run();

