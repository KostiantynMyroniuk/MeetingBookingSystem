using MeetingBookingSystem.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddIdentityServices();

builder.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

await app.SeedRolesAsync();

app.Run();

