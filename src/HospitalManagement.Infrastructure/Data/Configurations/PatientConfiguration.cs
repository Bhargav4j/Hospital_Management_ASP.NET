using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Patient entity
/// </summary>
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    /// <summary>
    /// Configures the Patient entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        // Table name
        builder.ToTable("Patients");

        // Primary key
        builder.HasKey(p => p.PatientID);

        // Properties
        builder.Property(p => p.PatientID)
            .HasColumnName("PatientID")
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasColumnName("Email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Password)
            .HasColumnName("Password")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Address)
            .HasColumnName("Address")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .HasColumnName("BirthDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasColumnName("Gender")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(p => p.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .HasColumnType("datetime");

        builder.Property(p => p.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(p => p.Email)
            .IsUnique()
            .HasDatabaseName("IX_Patients_Email");

        builder.HasIndex(p => p.Phone)
            .HasDatabaseName("IX_Patients_Phone");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Patients_IsActive");

        // Relationships
        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_Patient");
    }
}
