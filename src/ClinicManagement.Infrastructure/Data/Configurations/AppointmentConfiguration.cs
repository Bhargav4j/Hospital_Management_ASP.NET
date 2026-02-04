using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Appointment
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AppointmentDate)
            .IsRequired();

        builder.Property(a => a.AppointmentTime)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.Symptoms)
            .HasMaxLength(1000);

        builder.Property(a => a.Diagnosis)
            .HasMaxLength(1000);

        builder.Property(a => a.Prescription)
            .HasMaxLength(2000);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(a => a.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate });
        builder.HasIndex(a => new { a.PatientId, a.AppointmentDate });
    }
}
