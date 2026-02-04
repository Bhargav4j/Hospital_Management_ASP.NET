using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Data;

/// <summary>
/// Application database context for Hospital Management System
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly ILogger<ApplicationDbContext>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class
    /// </summary>
    /// <param name="options">Database context options</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with logger
    /// </summary>
    /// <param name="options">Database context options</param>
    /// <param name="logger">Logger instance</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ILogger<ApplicationDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets or sets the Patients DbSet
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Doctors DbSet
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Appointments DbSet
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Departments DbSet
    /// </summary>
    public DbSet<Department> Departments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the OtherStaff DbSet
    /// </summary>
    public DbSet<OtherStaff> OtherStaff { get; set; } = null!;

    /// <summary>
    /// Gets or sets the FreeSlots DbSet
    /// </summary>
    public DbSet<FreeSlot> FreeSlots { get; set; } = null!;

    /// <summary>
    /// Configures the model and relationships using Fluent API
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            _logger?.LogInformation("Configuring database model");

            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new OtherStaffConfiguration());
            modelBuilder.ApplyConfiguration(new FreeSlotConfiguration());

            // Seed data
            SeedData(modelBuilder);

            _logger?.LogInformation("Database model configuration completed successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while configuring database model");
            throw;
        }
    }

    /// <summary>
    /// Seeds initial data into the database
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    private void SeedData(ModelBuilder modelBuilder)
    {
        _logger?.LogInformation("Seeding initial data");

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DeptNo = 1,
                DeptName = "Cardiology",
                Description = "Department specializing in heart and cardiovascular system disorders",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 2,
                DeptName = "Neurology",
                Description = "Department specializing in nervous system disorders",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 3,
                DeptName = "Orthopedics",
                Description = "Department specializing in musculoskeletal system disorders",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 4,
                DeptName = "Pediatrics",
                Description = "Department specializing in medical care of infants, children, and adolescents",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 5,
                DeptName = "General Medicine",
                Description = "Department providing general medical consultation and treatment",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 6,
                DeptName = "Dermatology",
                Description = "Department specializing in skin, hair, and nail disorders",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 7,
                DeptName = "Ophthalmology",
                Description = "Department specializing in eye and vision care",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            },
            new Department
            {
                DeptNo = 8,
                DeptName = "ENT",
                Description = "Department specializing in ear, nose, and throat disorders",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedBy = "System"
            }
        );

        _logger?.LogInformation("Initial data seeding completed");
    }

    /// <summary>
    /// Saves changes to the database asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The number of state entries written to the database</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger?.LogError(ex, "Database update error occurred while saving changes");
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
    }
}
