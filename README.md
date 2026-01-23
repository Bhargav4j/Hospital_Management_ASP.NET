# Clinic Management System

## Overview
This is a modern .NET 8 Clinic Management System migrated from ASP.NET Web Forms 4.5.2. The application follows clean architecture principles and uses Razor Pages for the UI layer.

## Architecture
The solution follows Clean Architecture with four main layers:
- **Domain Layer**: Contains entities, interfaces, enums, and domain logic
- **Application Layer**: Contains services, DTOs, and business logic implementation
- **Infrastructure Layer**: Contains EF Core DbContext, repositories, and data access
- **Web Layer**: Razor Pages UI with ViewModels

## Technologies Used
- .NET 8
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0.0
- SQL Server
- Serilog for logging
- Bootstrap 5 for UI

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or SQL Server Express)

## Setup Instructions

### 1. Database Setup
Update the connection string in src/ClinicManagement.Web/appsettings.json

### 2. Run Migrations
```bash
cd src/ClinicManagement.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../ClinicManagement.Web
dotnet ef database update --startup-project ../ClinicManagement.Web
```

### 3. Build the Solution
```bash
dotnet build
```

### 4. Run the Application
```bash
cd src/ClinicManagement.Web
dotnet run
```

## Build Verification
✅ Build Status: SUCCESS
- All projects compile without errors
- 0 Warnings
- 0 Errors

## License
Copyright © 2026 - Clinic Management System
