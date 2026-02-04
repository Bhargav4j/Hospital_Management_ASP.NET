# Clinic Management System - .NET 8

A modern clinic management system migrated from ASP.NET Web Forms to .NET 8 using clean architecture principles.

## Migration Information

This application was successfully migrated from ASP.NET Web Forms 4.5.2 to .NET 8. The original application was a fully featured Clinic Management System with a well-designed database schema. The migration maintains all original functionality while modernizing the architecture and technology stack.

## Architecture

This application follows clean architecture with four main layers:

- **Domain Layer**: Contains entities, interfaces, enums, and business logic
- **Application Layer**: Contains application services, DTOs, and validation
- **Infrastructure Layer**: Contains data access, repositories, and external service implementations
- **Web Layer**: ASP.NET Core Razor Pages UI

## Technology Stack

- .NET 8
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0
- SQL Server
- Serilog for logging
- Bootstrap 5 for UI

## Features

### Patient Portal
- Patient registration and login
- View available doctors by specialization
- Book appointments with doctors
- View current and pending appointments
- View treatment history
- View bills history
- Submit feedback and ratings

### Doctor Portal
- View pending appointments
- Access patient medical records
- Update treatment history and prescriptions
- Generate bills for completed appointments
- View patient history

### Admin Portal
- Register new doctors
- Add staff members
- Manage clinic locations
- View system statistics
- Manage all users

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Install .NET 8 SDK**
   Download from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

2. **Install SQL Server**
   - [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
   - [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

3. **Setup Database**

   Open SQL Server Management Studio and connect:
   ```
   Server name: .\SQLEXPRESS
   Authentication: Windows Authentication
   ```

   Execute the following SQL files from the `Database Files` folder in order:
   - Schema.sql (creates database and tables)
   - Admin.sql
   - Doctor.sql
   - Patient.sql
   - Signup.sql
   - Insertions.sql (test data with login credentials)

4. **Update Connection String**

   Edit `src/ClinicManagement.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=DBProject;Integrated Security=True;TrustServerCertificate=True"
     }
   }
   ```

5. **Build the Solution**
   ```bash
   dotnet build ClinicManagement.sln
   ```

6. **Run the Application**
   ```bash
   cd src/ClinicManagement.Web
   dotnet run
   ```

7. **Access the Application**

   Open your browser and navigate to `https://localhost:5001`

## Project Structure

```
ClinicManagement/
├── src/
│   ├── ClinicManagement.Domain/          # Domain entities and interfaces
│   ├── ClinicManagement.Application/     # Application services and DTOs
│   ├── ClinicManagement.Infrastructure/  # Data access and repositories
│   └── ClinicManagement.Web/            # Web UI (Razor Pages)
├── tests/
│   ├── ClinicManagement.UnitTests/
│   └── ClinicManagement.IntegrationTests/
├── docs/                                 # Documentation
└── Database Files/                       # SQL scripts
```

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.5.2 to .NET 8. Key changes include:

### Configuration
- Web.config replaced with appsettings.json
- Connection strings moved to configuration
- Removed legacy compilation settings

### UI Framework
- Web Forms (.aspx) replaced with Razor Pages (.cshtml)
- Master pages replaced with _Layout.cshtml
- ViewState replaced with modern state management
- Server controls replaced with HTML helpers and Tag Helpers

### Data Access
- ADO.NET with manual connection management replaced with EF Core
- Stored procedure calls replaced with LINQ queries
- DataSet/DataTable replaced with strongly-typed entities
- Synchronous operations replaced with async/await

### Architecture
- Monolithic DAL replaced with clean architecture
- Introduced repository pattern
- Added dependency injection
- Separated concerns across layers

### Security
- Passwords are hashed using SHA256
- Session-based authentication
- HTTPS enforced
- CSRF protection enabled by default
- SQL injection prevention through EF Core

## Test Credentials

Use the credentials from the Insertions.sql file in the Database Files folder to test different user roles.

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

Original application developed as a final project for "Application Development with .NET and Web Services CS624" at Pace University.

## Support

For issues and questions, please open an issue in the GitHub repository.
