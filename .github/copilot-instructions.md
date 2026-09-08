## Project Overview
This repository contains the solution for the Meeting Room Booking System with Concurrency Control.
The core focus of this system is to handle simultaneous booking requests reliably, provide real-time status updates, and deploy to Azure.

## Technology Stack
- **Backend:** .NET 10, ASP.NET Core (C#)
- **Database:** Azure SQL Database via Entity Framework Core (EF Core)
- **Real-time:** Azure SignalR Service
- **Testing:** xUnit

## Architectural & Coding Guidelines

### 1. Concurrency Control
- **Strict Requirement:** Never use a naive "check if free, then insert" approach. 
- **Implementation Strategy:** Use an explicit concurrency control mechanism. The recommended approach is **Optimistic Concurrency** via EF Core.
  - Add a `[Timestamp]` / `RowVersion` column to the target entities.
  - Catch `DbUpdateConcurrencyException`.
  - On conflict, return a clean `409 Conflict` HTTP response with a clear message. Do NOT return a `500 Server Error` and never silently overwrite existing bookings.

### 2. Real-Time Updates (SignalR)
- All state changes to a resource's time slot must trigger a broadcast via Azure SignalR Service.
- Ensure the frontend receives these updates and reflects the UI changes immediately without requiring a page refresh.
- Scope broadcasts to users viewing the specific resource if possible, or broadcast globally if the user base is small.

### 3. Authentication & Roles
- Ensure role-based access control (RBAC) is enforced at the minimal api level using require authorization roles.
- **Roles:**
  - `User`: Can view resources, schedules, and book slots.
  - `Admin`: Can create, edit, remove resources, and view all bookings across users.

### 4. Automated Testing
- **Concurrency Test:** You must write automated tests that simulate race conditions.
  - Use `Task.WhenAll` to send multiple simultaneous HTTP POST requests to book the exact same timeslot.
  - Assert that exactly **one** request succeeds and the others fail with a concurrency conflict.

### 5. Git & Commit Messages
- **Atomic Commits:** Generate code in logical, atomic chunks. 
- **Commit Message Format:** When suggesting commit messages, always include a short description of **what** changed and **why**.
  - Example: `feat: add RowVersion to Booking entity - required for optimistic concurrency control`