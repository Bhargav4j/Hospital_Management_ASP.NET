using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Bill entity
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    /// <summary>
    /// Configures the Bill entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bill");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ConsultationFee)
            .HasPrecision(18, 2);

        builder.Property(b => b.MedicineCost)
            .HasPrecision(18, 2);

        builder.Property(b => b.TestCharges)
            .HasPrecision(18, 2);

        builder.Property(b => b.OtherCharges)
            .HasPrecision(18, 2);

        builder.Property(b => b.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(b => b.BillDate);
        builder.HasIndex(b => b.IsPaid);
    }
}
