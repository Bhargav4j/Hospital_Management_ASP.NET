using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

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
            .HasMaxLength(20);
        
        builder.Property(d => d.Address)
            .HasMaxLength(500);
        
        builder.Property(d => d.Gender)
            .HasMaxLength(10);
        
        builder.Property(d => d.Specialization)
            .HasMaxLength(100);
        
        builder.Property(d => d.Qualification)
            .HasMaxLength(200);
        
        builder.Property(d => d.Salary)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(d => d.ChargesPerVisit)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(d => d.CreatedDate)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        
        builder.HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DeptNo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
