using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Doctor entity
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctor");

        builder.HasKey(d => d.DoctorID);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(d => d.Email)
            .IsUnique();

        builder.Property(d => d.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Gender)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(d => d.Salary)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.ChargesPerVisit)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.Specialization)
            .HasMaxLength(200);

        builder.Property(d => d.Qualification)
            .HasMaxLength(500);

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DeptNo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Feedbacks)
            .WithOne(f => f.Doctor)
            .HasForeignKey(f => f.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
