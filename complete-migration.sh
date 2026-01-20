#!/bin/bash
# Complete Hospital Management System Migration Script
# This script generates all remaining files for the .NET 8 migration

BASE_DIR="/modernize-data/studio-data/TNT1001/APP1117/transformed-code/302/studio-workspace/hospitalmgmt1901"

# Function to create Web layer Program.cs
create_program_cs() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/Program.cs" << 'EOF'
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Extensions;
using HospitalManagement.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/hospital-management-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Infrastructure services
builder.Services.AddInfrastructureServices();

// Add Application services
builder.Services.AddApplicationServices();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

try
{
    Log.Information("Starting Hospital Management System");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
EOF
}

# Function to create appsettings.json
create_appsettings() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/appsettings.json" << 'EOF'
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DBProject;Integrated Security=True;TrustServerCertificate=True"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  },
  "AllowedHosts": "*"
}
EOF

    cat > "$BASE_DIR/src/HospitalManagement.Web/appsettings.Development.json" << 'EOF'
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug"
    }
  }
}
EOF
}

# Function to create _ViewImports.cshtml
create_view_imports() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/Pages/_ViewImports.cshtml" << 'EOF'
@using HospitalManagement.Web
@using HospitalManagement.Web.ViewModels
@namespace HospitalManagement.Web.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
EOF
}

# Function to create _ViewStart.cshtml
create_view_start() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/Pages/_ViewStart.cshtml" << 'EOF'
@{
    Layout = "_Layout";
}
EOF
}

# Function to create _Layout.cshtml
create_layout() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/Pages/Shared/_Layout.cshtml" << 'EOF'
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Hospital Management System</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-area="" asp-page="/Index">Hospital Management</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2026 - Hospital Management System
        </div>
    </footer>

    <script src="https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
EOF
}

# Function to create Index page
create_index_page() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/Pages/Index.cshtml" << 'EOF'
@page
@model IndexModel
@{
    ViewData["Title"] = "Home";
}

<div class="text-center">
    <h1 class="display-4">Welcome to Hospital Management System</h1>
    <p>Modern .NET 8 application for managing hospital operations</p>

    <div class="row mt-5">
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Patient Portal</h5>
                    <p class="card-text">Access patient information and services</p>
                    <a href="/Patient/Index" class="btn btn-primary">Go to Patients</a>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Doctor Portal</h5>
                    <p class="card-text">Manage appointments and patient care</p>
                    <a href="/Doctor/Index" class="btn btn-primary">Go to Doctors</a>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Admin Portal</h5>
                    <p class="card-text">System administration and reports</p>
                    <a href="/Admin/Index" class="btn btn-primary">Go to Admin</a>
                </div>
            </div>
        </div>
    </div>
</div>
EOF

    cat > "$BASE_DIR/src/HospitalManagement.Web/Pages/Index.cshtml.cs" << 'EOF'
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
EOF
}

# Function to create CSS
create_css() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/wwwroot/css/site.css" << 'EOF'
html {
  font-size: 14px;
}

@media (min-width: 768px) {
  html {
    font-size: 16px;
  }
}

.btn-primary {
  color: #fff;
  background-color: #1b6ec2;
  border-color: #1861ac;
}

.footer {
  position: absolute;
  bottom: 0;
  width: 100%;
  white-space: nowrap;
  line-height: 60px;
}
EOF
}

# Function to create JS
create_js() {
    cat > "$BASE_DIR/src/HospitalManagement.Web/wwwroot/js/site.js" << 'EOF'
// Site-specific JavaScript
console.log('Hospital Management System loaded');
EOF
}

# Function to create Infrastructure extensions
create_infrastructure_extensions() {
    cat > "$BASE_DIR/src/HospitalManagement.Infrastructure/Extensions/ServiceCollectionExtensions.cs" << 'EOF'
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<ITreatmentHistoryRepository, TreatmentHistoryRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();

        return services;
    }
}
EOF
}

# Function to create Application extensions
create_application_extensions() {
    cat > "$BASE_DIR/src/HospitalManagement.Application/Extensions/ServiceCollectionExtensions.cs" << 'EOF'
using HospitalManagement.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register services would go here
        // services.AddScoped<IPatientService, PatientService>();
        // etc.

        return services;
    }
}
EOF
}

# Function to create AutoMapper profile
create_automapper_profile() {
    cat > "$BASE_DIR/src/HospitalManagement.Application/Mappings/MappingProfile.cs" << 'EOF'
using AutoMapper;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>();
        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<PatientUpdateDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>();
        CreateMap<DoctorCreateDto, Doctor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<DoctorUpdateDto, Doctor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<AppointmentUpdateDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Bill mappings
        CreateMap<Bill, BillDto>();
        CreateMap<BillCreateDto, Bill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<BillUpdateDto, Bill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Feedback mappings
        CreateMap<Feedback, FeedbackDto>();
        CreateMap<FeedbackCreateDto, Feedback>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<FeedbackUpdateDto, Feedback>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TreatmentHistory mappings
        CreateMap<TreatmentHistory, TreatmentHistoryDto>();
        CreateMap<TreatmentHistoryCreateDto, TreatmentHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<TreatmentHistoryUpdateDto, TreatmentHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Admin mappings
        CreateMap<Admin, AdminDto>();
        CreateMap<AdminCreateDto, Admin>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<AdminUpdateDto, Admin>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
EOF
}

# Execute all functions
echo "Creating Program.cs..."
create_program_cs

echo "Creating appsettings files..."
create_appsettings

echo "Creating View files..."
create_view_imports
create_view_start
create_layout
create_index_page

echo "Creating static files..."
create_css
create_js

echo "Creating Infrastructure extensions..."
create_infrastructure_extensions

echo "Creating Application extensions..."
create_application_extensions

echo "Creating AutoMapper profile..."
create_automapper_profile

echo "Migration script completed!"
EOF

chmod +x /modernize-data/studio-data/TNT1001/APP1117/transformed-code/302/studio-workspace/hospitalmgmt1901/complete-migration.sh
