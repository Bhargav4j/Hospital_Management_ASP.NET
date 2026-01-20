using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Data;

/// <summary>
/// Database context for the Hospital Management System
/// </summary>
public class HospitalDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the HospitalDbContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for Patients
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = null!;

    /// <summary>
    /// DbSet for Doctors
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; } = null!;

    /// <summary>
    /// DbSet for Appointments
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; } = null!;

    /// <summary>
    /// DbSet for Treatment Histories
    /// </summary>
    public DbSet<TreatmentHistory> TreatmentHistories { get; set; } = null!;

    /// <summary>
    /// DbSet for Bills
    /// </summary>
    public DbSet<Bill> Bills { get; set; } = null!;

    /// <summary>
    /// DbSet for Feedback
    /// </summary>
    public DbSet<Feedback> Feedbacks { get; set; } = null!;

    /// <summary>
    /// DbSet for Admins
    /// </summary>
    public DbSet<Admin> Admins { get; set; } = null!;

    /// <summary>
    /// Configures the model
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalDbContext).Assembly);
    }
}
