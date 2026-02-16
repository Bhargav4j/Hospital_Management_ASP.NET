using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class OtherStaffConfiguration : IEntityTypeConfiguration<OtherStaff>
{
    public void Configure(EntityTypeBuilder<OtherStaff> builder)
    {
        builder.ToTable("OtherStaff");
        
        builder.HasKey(s => s.StaffID);
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(s => s.Phone)
            .HasMaxLength(20);
        
        builder.Property(s => s.Address)
            .HasMaxLength(500);
        
        builder.Property(s => s.Gender)
            .HasMaxLength(10);
        
        builder.Property(s => s.Designation)
            .HasMaxLength(100);
        
        builder.Property(s => s.Qualification)
            .HasMaxLength(200);
        
        builder.Property(s => s.Salary)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(s => s.CreatedDate)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
    }
}
