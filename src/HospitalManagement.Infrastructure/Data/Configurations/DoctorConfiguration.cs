using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Doctor entity
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    /// <summary>
    /// Configures the Doctor entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        // Table name
        builder.ToTable("Doctors");

        // Primary key
        builder.HasKey(d => d.DoctorID);

        // Properties
        builder.Property(d => d.DoctorID)
            .HasColumnName("DoctorID")
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Email)
            .HasColumnName("Email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Password)
            .HasColumnName("Password")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.Address)
            .HasColumnName("Address")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.BirthDate)
            .HasColumnName("BirthDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(d => d.Gender)
            .HasColumnName("Gender")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(d => d.DeptNo)
            .HasColumnName("DeptNo")
            .IsRequired();

        builder.Property(d => d.Experience)
            .HasColumnName("Experience")
            .IsRequired();

        builder.Property(d => d.Salary)
            .HasColumnName("Salary")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.ChargesPerVisit)
            .HasColumnName("ChargesPerVisit")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.Specialization)
            .HasColumnName("Specialization")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Qualification)
            .HasColumnName("Qualification")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.ReputationIndex)
            .HasColumnName("ReputationIndex")
            .HasColumnType("real")
            .IsRequired()
            .HasDefaultValue(0.0f);

        builder.Property(d => d.PatientsTreated)
            .HasColumnName("PatientsTreated")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(d => d.Status)
            .HasColumnName("Status")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(d => d.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .HasColumnType("datetime");

        builder.Property(d => d.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(d => d.Email)
            .IsUnique()
            .HasDatabaseName("IX_Doctors_Email");

        builder.HasIndex(d => d.DeptNo)
            .HasDatabaseName("IX_Doctors_DeptNo");

        builder.HasIndex(d => d.Specialization)
            .HasDatabaseName("IX_Doctors_Specialization");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Doctors_IsActive");

        builder.HasIndex(d => d.Status)
            .HasDatabaseName("IX_Doctors_Status");

        // Relationships
        builder.HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DeptNo)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Doctors_Department");

        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_Doctor");
    }
}
