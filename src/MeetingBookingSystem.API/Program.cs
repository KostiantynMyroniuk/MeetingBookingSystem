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

await app.MigrateDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabaseDefaults();

app.MapGroup("/api").MapIdentityApi<ApplicationUser>();
app.MapGroup("/api").MapLogoutApi<ApplicationUser>();

app.MapMeetingRoomsApi();
app.MapTimeSlotsApi();
app.MapBookingApi();
app.MapCurrentUserApi();

app.MapHub<MeetingRoomHub>("/hubs/meeting-rooms");

app.Run();


