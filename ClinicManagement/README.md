# Clinic Management System - .NET 8 Migration

This project has been successfully migrated from ASP.NET Web Forms to .NET 8 with Razor Pages, following clean architecture principles.

## Project Structure

```
ClinicManagement/
├── src/
│   ├── ClinicManagement.Domain/         # Domain entities, interfaces, enums
│   ├── ClinicManagement.Application/    # Business logic, services, DTOs, mappings
│   ├── ClinicManagement.Infrastructure/ # Data access, EF Core, repositories
│   └── ClinicManagement.Web/           # Razor Pages UI
├── tests/
│   └── ClinicManagement.UnitTests/
└── docs/
```

## Technology Stack

- .NET 8.0
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0
- SQL Server
- AutoMapper 12.0
- Serilog for logging
- Bootstrap 5 for UI

## Key Features

### Patient Portal
- View available doctors
- Book appointments
- View appointment history
- Update profile

### Doctor Portal
- View appointments
- Update patient records
- Manage prescriptions and bills

### Admin Portal
- Manage patients
- Manage doctors
- Manage departments
- Manage staff
- View all appointments

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Configuration

1. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ClinicManagement;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

2. Run database migrations:
```bash
cd src/ClinicManagement.Web
dotnet ef migrations add InitialCreate --project ../ClinicManagement.Infrastructure
dotnet ef database update --project ../ClinicManagement.Infrastructure
```

3. Run the application:
```bash
dotnet run
```

### Default Admin Login
- Email: admin@clinic.com
- Password: admin123

## Database Schema

The application uses the following main entities:
- **Patient**: Patient information and credentials
- **Doctor**: Doctor profiles and specializations
- **Department**: Medical departments
- **Appointment**: Appointment bookings and medical records
- **OtherStaff**: Administrative and support staff

## Architecture

### Clean Architecture Layers

1. **Domain Layer**: Contains entities, enums, and interfaces (no dependencies)
2. **Application Layer**: Business logic, services, DTOs, AutoMapper profiles
3. **Infrastructure Layer**: EF Core DbContext, repository implementations, configurations
4. **Web Layer**: Razor Pages, session management, presentation logic

### Design Patterns Used
- Repository Pattern
- Dependency Injection
- Service Layer Pattern
- DTO Pattern
- AutoMapper for object mapping

## Migration Notes

### Changes from Web Forms
- **Session State**: Migrated to ASP.NET Core Session
- **Authentication**: Simplified authentication using session-based approach
- **Data Access**: Migrated from ADO.NET to Entity Framework Core
- **UI**: Converted from ASPX to Razor Pages with Bootstrap 5
- **Validation**: Using Data Annotations and built-in validation

### What Was Migrated
✅ All patient management features
✅ All doctor management features
✅ Appointment booking and management
✅ Department management
✅ Authentication and authorization
✅ Session management
✅ Database operations

### Production Considerations

Before deploying to production:
1. ✅ Implement proper password hashing (currently uses SHA256)
2. ✅ Add comprehensive input validation
3. ✅ Implement proper authentication/authorization (consider ASP.NET Core Identity)
4. ✅ Add logging and monitoring
5. ✅ Configure HTTPS
6. ✅ Add rate limiting
7. ✅ Implement comprehensive error handling
8. ✅ Add unit and integration tests

## Building and Running

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
cd src/ClinicManagement.Web
dotnet run

# Run tests
cd tests/ClinicManagement.UnitTests
dotnet test
```

## API Endpoints (Pages)

### Public
- `/` - Home page
- `/Login` - Login page
- `/Logout` - Logout

### Patient Portal
- `/Patient/Index` - Patient dashboard
- `/Patient/ViewDoctors` - Browse doctors
- `/Patient/BookAppointment` - Book appointment
- `/Patient/Appointments` - View appointments

### Doctor Portal
- `/Doctor/Index` - Doctor dashboard
- `/Doctor/Appointments` - View and manage appointments
- `/Doctor/UpdateAppointment` - Update appointment details

### Admin Portal
- `/Admin/Index` - Admin dashboard
- `/Admin/Patients/Index` - Manage patients
- `/Admin/Doctors/Index` - Manage doctors
- `/Admin/Departments/Index` - Manage departments
- `/Admin/Staff/Index` - Manage staff

## License

Copyright © 2024 Clinic Management System

## Support

For issues and questions, please create an issue in the repository.
