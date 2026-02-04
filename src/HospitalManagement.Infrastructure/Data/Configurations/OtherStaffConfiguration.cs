using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for OtherStaff entity
/// </summary>
public class OtherStaffConfiguration : IEntityTypeConfiguration<OtherStaff>
{
    /// <summary>
    /// Configures the OtherStaff entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<OtherStaff> builder)
    {
        // Table name
        builder.ToTable("OtherStaff");

        // Primary key
        builder.HasKey(s => s.StaffID);

        // Properties
        builder.Property(s => s.StaffID)
            .HasColumnName("StaffID")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.Address)
            .HasColumnName("Address")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.BirthDate)
            .HasColumnName("BirthDate")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(s => s.Gender)
            .HasColumnName("Gender")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(s => s.Designation)
            .HasColumnName("Designation")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Qualification)
            .HasColumnName("Qualification")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.Salary)
            .HasColumnName("Salary")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(s => s.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .HasColumnType("datetime");

        builder.Property(s => s.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(s => s.Phone)
            .HasDatabaseName("IX_OtherStaff_Phone");

        builder.HasIndex(s => s.Designation)
            .HasDatabaseName("IX_OtherStaff_Designation");

        builder.HasIndex(s => s.IsActive)
            .HasDatabaseName("IX_OtherStaff_IsActive");
    }
}
