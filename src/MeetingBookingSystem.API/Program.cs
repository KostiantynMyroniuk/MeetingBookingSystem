using MeetingBookingSystem.API.Apis;
using MeetingBookingSystem.API.Apis.Hubs;
using MeetingBookingSystem.API.Extensions;
using MeetingBookingSystem.API.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddIdentityServices();
builder.AddApplicationServices();
builder.AddPersistence();
builder.AddSignalR();
builder.AddCors();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("BookingFrontend");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabaseDefaults();

app.MapGroup("/api")
    .MapIdentityApi<ApplicationUser>();

app.MapMeetingRoomsApi();
app.MapTimeSlotsApi();
app.MapBookingApi();

app.MapHub<MeetingRoomHub>("/hubs/meeting-rooms");

app.Run();

