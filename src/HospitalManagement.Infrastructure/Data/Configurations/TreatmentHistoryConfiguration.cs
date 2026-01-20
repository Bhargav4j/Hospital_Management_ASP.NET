using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for TreatmentHistory entity
/// </summary>
public class TreatmentHistoryConfiguration : IEntityTypeConfiguration<TreatmentHistory>
{
    /// <summary>
    /// Configures the TreatmentHistory entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<TreatmentHistory> builder)
    {
        builder.ToTable("TreatmentHistory");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Diagnosis)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(t => t.Prescription)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(t => t.MedicalTests)
            .HasMaxLength(1000);

        builder.Property(t => t.Notes)
            .HasMaxLength(2000);

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(t => t.TreatmentDate);
    }
}
