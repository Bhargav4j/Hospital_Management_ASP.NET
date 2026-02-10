using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Bill entity
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();

        builder.Property(b => b.PatientId).IsRequired();
        builder.Property(b => b.DoctorId).IsRequired();

        builder.Property(b => b.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.BillDate).IsRequired();

        builder.Property(b => b.Description).HasMaxLength(1000);

        builder.Property(b => b.IsPaid)
            .HasDefaultValue(false);

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(200);
    }
}
