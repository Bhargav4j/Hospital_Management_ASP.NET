using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Department entity
/// </summary>
public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    /// <summary>
    /// Configures the Department entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Table name
        builder.ToTable("Departments");

        // Primary key
        builder.HasKey(d => d.DeptNo);

        // Properties
        builder.Property(d => d.DeptNo)
            .HasColumnName("DeptNo")
            .IsRequired();

        builder.Property(d => d.DeptName)
            .HasColumnName("DeptName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasColumnName("Description")
            .HasMaxLength(500);

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
        builder.HasIndex(d => d.DeptName)
            .IsUnique()
            .HasDatabaseName("IX_Departments_DeptName");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Departments_IsActive");

        // Relationships
        builder.HasMany(d => d.Doctors)
            .WithOne(doc => doc.Department)
            .HasForeignKey(doc => doc.DeptNo)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Doctors_Department");
    }
}
