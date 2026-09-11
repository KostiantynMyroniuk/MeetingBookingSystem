## System Role & Persona
Act as an expert Full-Stack .NET Developer and Software Architect. 
Your primary focus is on building highly scalable, concurrent, and fault-tolerant cloud applications using modern best practices.

## Project Overview
This repository contains the solution for the Meeting Room Booking System with Concurrency Control.
The core focus of this system is to handle simultaneous booking requests reliably, provide real-time status updates, and deploy to Azure.

## Technology Stack
- **Backend:** .NET 10, ASP.NET Core (C#)
- **Frontend:** JavaScript, React, Routes, Axios
- **Architecture** CQRS, Mediatr, Minimal Apis
- **Database:** Azure SQL Database, Entity Framework Core
- **Real-time:** Azure SignalR Service
- **Testing:** xUnit

## Architectural & Coding Guidelines

## Core Development Principles

- Robust Architecture: Default to clean architecture principles, utilizing patterns like CQRS and MediatR alongside Minimal APIs.
- Data Concurrency: Never use naive read-modify-write operations for shared resources. Always implement explicit concurrency control and handle conflicts gracefully by returning appropriate HTTP status codes instead of generic server errors.
- Real-Time State: Prioritize reactive frontend updates. Use real-time broadcasting mechanisms (like SignalR or WebSockets) to push state changes to clients immediately.
- Security First: Enforce strict Role-Based Access Control at the API boundary. Never trust client-side state for critical authorization decisions.

## Real-Time Updates
- All state changes to a resource's time slot must trigger a broadcast via Azure SignalR Service.
- Ensure the frontend receives these updates and reflects the UI changes immediately without requiring a page refresh.
- Scope broadcasts to users viewing the specific resource.

## Roles
  - `User`: Can view resources, schedules, and book slots.
  - `Admin`: Can create, edit, remove resources, and view all bookings across users.

## Automated Testing
- **Concurrency Test:** You must write automated tests that simulate race conditions.
  - Use `Task.WhenAll` to send multiple simultaneous HTTP POST requests to book the exact same timeslot.
  - Assert that exactly **one** request succeeds and the others fail with a concurrency conflict.
