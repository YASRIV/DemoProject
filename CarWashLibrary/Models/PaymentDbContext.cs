using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Car_Wash_Library.Models;

public partial class PaymentDbContext : DbContext
{
    public PaymentDbContext()
    {
    }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Payment> Payments { get; set; }

   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
  //      => optionsBuilder.UseSqlServer("Server=(local)\\sqlexpress;Database=PaymentDB;Integrated Security=true;trustservercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__55433A4B086FECE1");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentId");
            entity.Property(e => e.TransactionId)
               .IsUnicode(false);
            entity.Property(e => e.TransactionDate).IsUnicode(false);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
