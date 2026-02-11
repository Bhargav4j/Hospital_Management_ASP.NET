using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.ToTable("MedicalHistories");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Diagnosis)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.Treatment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.VisitDate)
            .IsRequired();

        builder.Property(m => m.Notes)
            .HasMaxLength(2000);

        builder.Property(m => m.IsActive)
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(m => m.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(m => m.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(m => m.Patient)
            .WithMany(u => u.MedicalHistories)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
