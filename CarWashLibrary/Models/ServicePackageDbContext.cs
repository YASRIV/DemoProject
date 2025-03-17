using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Car_Wash_Library.Models;

public partial class ServicePackageDbContext : DbContext
{
    public ServicePackageDbContext()
    {
    }

    public ServicePackageDbContext(DbContextOptions<ServicePackageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServicePlan> Services { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(local)\\sqlexpress;Database=ServicePackageDB;Integrated Security=true;trustservercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServicePlan>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A4102B4BF");

            entity.Property(e => e.ServiceDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
