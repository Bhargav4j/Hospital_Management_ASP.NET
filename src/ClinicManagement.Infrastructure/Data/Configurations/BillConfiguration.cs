using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Bill
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.BillDate)
            .IsRequired();

        builder.Property(b => b.IsPaid)
            .HasDefaultValue(false);

        builder.Property(b => b.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(b => b.PatientId);
        builder.HasIndex(b => b.BillDate);
    }
}
