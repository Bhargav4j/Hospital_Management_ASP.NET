using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data;

public partial class ClinicManagementDbContext : DbContext
{
    public ClinicManagementDbContext(DbContextOptions<ClinicManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<appointment> appointments { get; set; }

    public virtual DbSet<appointment_view> appointment_views { get; set; }

    public virtual DbSet<department> departments { get; set; }

    public virtual DbSet<department_view> department_views { get; set; }

    public virtual DbSet<deptinfo> deptinfos { get; set; }

    public virtual DbSet<doctor> doctors { get; set; }

    public virtual DbSet<income> incomes { get; set; }

    public virtual DbSet<logintable> logintables { get; set; }

    public virtual DbSet<otherstaff> otherstaffs { get; set; }

    public virtual DbSet<patient> patients { get; set; }

    public virtual DbSet<patient_view> patient_views { get; set; }

    public virtual DbSet<staff_view> staff_views { get; set; }

    public virtual DbSet<total_doctor> total_doctors { get; set; }

    public virtual DbSet<total_patient> total_patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<appointment>(entity =>
        {
            entity.HasKey(e => e.appointid).HasName("appointment_pkey");

            entity.HasOne(d => d.doctor).WithMany(p => p.appointments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_appt_doc");

            entity.HasOne(d => d.patient).WithMany(p => p.appointments).HasConstraintName("fk_appt_pat");
        });

        modelBuilder.Entity<appointment_view>(entity =>
        {
            entity.ToView("appointment_view", "dbo");
        });

        modelBuilder.Entity<department>(entity =>
        {
            entity.HasKey(e => e.deptno).HasName("pk_dept");

            entity.Property(e => e.deptno).ValueGeneratedNever();
        });

        modelBuilder.Entity<department_view>(entity =>
        {
            entity.ToView("department_view", "dbo");
        });

        modelBuilder.Entity<deptinfo>(entity =>
        {
            entity.ToView("deptinfo", "dbo");
        });

        modelBuilder.Entity<doctor>(entity =>
        {
            entity.HasKey(e => e.doctorid).HasName("pk_doc");

            entity.Property(e => e.doctorid).ValueGeneratedNever();
            entity.Property(e => e.patients_treated).HasDefaultValue(0);
            entity.Property(e => e.phone).IsFixedLength();

            entity.HasOne(d => d.deptnoNavigation).WithMany(p => p.doctors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_dept");
        });

        modelBuilder.Entity<income>(entity =>
        {
            entity.ToView("income", "dbo");
        });

        modelBuilder.Entity<logintable>(entity =>
        {
            entity.HasKey(e => e.loginid).HasName("logintable_pkey");
        });

        modelBuilder.Entity<otherstaff>(entity =>
        {
            entity.HasKey(e => e.staffid).HasName("otherstaff_pkey");

            entity.Property(e => e.phone).IsFixedLength();
        });

        modelBuilder.Entity<patient>(entity =>
        {
            entity.HasKey(e => e.patientid).HasName("pk_pat");

            entity.Property(e => e.patientid).ValueGeneratedNever();
            entity.Property(e => e.phone).IsFixedLength();

            entity.HasOne(d => d.patientNavigation).WithOne(p => p.patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pat_login");
        });

        modelBuilder.Entity<patient_view>(entity =>
        {
            entity.ToView("patient_view", "dbo");

            entity.Property(e => e.phone).IsFixedLength();
        });

        modelBuilder.Entity<staff_view>(entity =>
        {
            entity.ToView("staff_view", "dbo");
        });

        modelBuilder.Entity<total_doctor>(entity =>
        {
            entity.ToView("total_doctors", "dbo");
        });

        modelBuilder.Entity<total_patient>(entity =>
        {
            entity.ToView("total_patient", "dbo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

