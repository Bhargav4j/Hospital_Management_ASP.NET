using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for FreeSlot entity
/// </summary>
public class FreeSlotConfiguration : IEntityTypeConfiguration<FreeSlot>
{
    /// <summary>
    /// Configures the FreeSlot entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<FreeSlot> builder)
    {
        // Table name
        builder.ToTable("FreeSlots");

        // Primary key
        builder.HasKey(f => f.FreeSlotID);

        // Properties
        builder.Property(f => f.FreeSlotID)
            .HasColumnName("FreeSlotID")
            .IsRequired();

        builder.Property(f => f.DoctorID)
            .HasColumnName("DoctorID")
            .IsRequired();

        builder.Property(f => f.StartTime)
            .HasColumnName("StartTime")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(f => f.EndTime)
            .HasColumnName("EndTime")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(f => f.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .HasColumnType("datetime");

        builder.Property(f => f.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(f => f.DoctorID)
            .HasDatabaseName("IX_FreeSlots_DoctorID");

        builder.HasIndex(f => f.IsActive)
            .HasDatabaseName("IX_FreeSlots_IsActive");

        builder.HasIndex(f => new { f.DoctorID, f.StartTime, f.EndTime })
            .HasDatabaseName("IX_FreeSlots_Doctor_Time");

        // Relationships
        builder.HasOne(f => f.Doctor)
            .WithMany()
            .HasForeignKey(f => f.DoctorID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_FreeSlots_Doctor");

        builder.HasMany(f => f.Appointments)
            .WithOne(a => a.FreeSlot)
            .HasForeignKey(a => a.FreeSlotID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Appointments_FreeSlot");
    }
}
