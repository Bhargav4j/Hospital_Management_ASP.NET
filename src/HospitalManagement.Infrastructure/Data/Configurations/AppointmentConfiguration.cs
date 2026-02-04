using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Appointment entity
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    /// <summary>
    /// Configures the Appointment entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        // Table name
        builder.ToTable("Appointments");

        // Primary key
        builder.HasKey(a => a.AppointmentID);

        // Properties
        builder.Property(a => a.AppointmentID)
            .HasColumnName("AppointmentID")
            .IsRequired();

        builder.Property(a => a.PatientID)
            .HasColumnName("PatientID")
            .IsRequired();

        builder.Property(a => a.DoctorID)
            .HasColumnName("DoctorID")
            .IsRequired();

        builder.Property(a => a.FreeSlotID)
            .HasColumnName("FreeSlotID")
            .IsRequired();

        builder.Property(a => a.AppointmentDate)
            .HasColumnName("AppointmentDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.Status)
            .HasColumnName("Status")
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Pending");

        builder.Property(a => a.Disease)
            .HasColumnName("Disease")
            .HasMaxLength(500);

        builder.Property(a => a.Progress)
            .HasColumnName("Progress")
            .HasMaxLength(1000);

        builder.Property(a => a.Prescription)
            .HasColumnName("Prescription")
            .HasMaxLength(2000);

        builder.Property(a => a.IsPaid)
            .HasColumnName("IsPaid")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.FeedbackGiven)
            .HasColumnName("FeedbackGiven")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .HasColumnType("datetime");

        builder.Property(a => a.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(a => a.PatientID)
            .HasDatabaseName("IX_Appointments_PatientID");

        builder.HasIndex(a => a.DoctorID)
            .HasDatabaseName("IX_Appointments_DoctorID");

        builder.HasIndex(a => a.FreeSlotID)
            .HasDatabaseName("IX_Appointments_FreeSlotID");

        builder.HasIndex(a => a.AppointmentDate)
            .HasDatabaseName("IX_Appointments_AppointmentDate");

        builder.HasIndex(a => a.Status)
            .HasDatabaseName("IX_Appointments_Status");

        builder.HasIndex(a => a.IsActive)
            .HasDatabaseName("IX_Appointments_IsActive");

        // Composite indexes for common queries
        builder.HasIndex(a => new { a.DoctorID, a.AppointmentDate, a.Status })
            .HasDatabaseName("IX_Appointments_Doctor_Date_Status");

        builder.HasIndex(a => new { a.PatientID, a.AppointmentDate })
            .HasDatabaseName("IX_Appointments_Patient_Date");

        // Relationships
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_Patient");

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_Doctor");

        builder.HasOne(a => a.FreeSlot)
            .WithMany(f => f.Appointments)
            .HasForeignKey(a => a.FreeSlotID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_FreeSlot");
    }
}
