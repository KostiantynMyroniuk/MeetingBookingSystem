# Meeting Room Booking System

A booking system for a limited set of meeting rooms where multiple users may try to book the same time slot at the same time. The system guarantees a slot is never double-booked and reflects booking status to all viewers in real time via SignalR.

## Url: https://purple-glacier-091ed7403.6.azurestaticapps.net

### Default admin login

Seeded automatically on first run from configuration (default values for testing).

```
email:    admin@test.com
password: adminadmin
```

## Tech stack

- **Backend:** ASP.NET Core (.NET 10), Minimal APIs, CQRS via MediatR, Entity Framework Core, SQL Server
- **Frontend:** React 19, React Router, Axios, Vite, `@microsoft/signalr`
- **Real-time:** SignalR + Azure
- **Tests:** xUnit, Testcontainers (spins up a real, disposable SQL Server for the concurrency test)

## Roles

- **User** — view rooms and their schedules, book an available slot, view and cancel their own bookings.
- **Admin** — everything a User can do, plus create/edit/remove rooms and view all bookings across every user.

Regular users can self-register from the app's Register page.

## Booking concurrency control

A naive "check if free, then insert" is not used. Booking is protected by two layers, both explicit and deliberate:

1. **Optimistic concurrency (primary mechanism):** `MeetingRoomTimeSlot.RowVersion` is an EF Core concurrency token (SQL Server `rowversion`). Booking a slot updates its `IsBooked` flag in the same `SaveChanges` transaction that inserts the `Booking` row. If two requests race for the same slot, the second one's `UPDATE` fails its `RowVersion` check and EF throws `DbUpdateConcurrencyException`, which the handler maps to `409 Conflict`.
2. **Unique constraint (backstop):** `Booking.MeetingRoomTimeSlotId` has a unique index, so even a raw constraint violation (`DbUpdateException`) is also caught and mapped to `409 Conflict` — never a `500`.

Result: exactly one concurrent request succeeds (`200 OK`), the rest receive a clean `409 Conflict`, never a silent overwrite and never a server error.

### Automated concurrency test

`src/MeetingBookingSystem.Tests` fires 20 simultaneous `POST /api/bookings` requests at the same time slot and asserts exactly one succeeds. It spins up its own disposable SQL Server via Testcontainers, so it needs **Docker running** but nothing else pre-configured.

```bash
dotnet test src/MeetingBookingSystem.Tests
```

## Real-time updates

The backend hub is at `/hubs/meeting-rooms`. Clients join a group per room (`JoinGroup(roomId)`), and any booking or cancellation broadcasts a `SlotStatusChanged` event to everyone currently viewing that room's schedule — no page refresh needed.

By default this runs on local, in-process SignalR. To use Azure SignalR Service instead, set a connection string in configuration:

```json
"ConnectionStrings": {
  "AzureSignalR": "Endpoint=https://<your-service>.service.signalr.net;AccessKey=...;Version=1.0;"
}
```

## Running locally

### Prerequisites

- .NET 10 SDK
- Node.js (18+) and npm
- SQL Server reachable via the `BookingServiceDb` connection string in `appsettings.Development.json` (LocalDB works out of the box on Windows; on macOS/Linux, run SQL Server in Docker and point the connection string at it)

### Backend

```bash
cd src/MeetingBookingSystem.API
dotnet run
```

Runs migrations and seeds the `User`/`Admin` roles and the admin account automatically on startup (Development environment).

### Frontend

```bash
cd src/meetingbookingsystem.client
npm install
npm run dev
```

The Vite dev server proxies `/api` and `/hubs` to the backend (`http://localhost:5241` by default — see `vite.config.js`).

### Creating a room

Time slots are generated automatically when an admin creates a room: 9:00–18:00, hourly. No manual slot creation is needed.

## Repository / process notes

- Development on this repository was done with active use of Claude Code and GitHub Copilot.
- Commit history is kept atomic: each commit is one logical change with a description of what and why.
