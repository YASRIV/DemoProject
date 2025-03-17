using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Car_Wash_Library.Models;

public partial class WashersDbContext : DbContext
{
    public WashersDbContext()
    {
    }

    public WashersDbContext(DbContextOptions<WashersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Washer> Washers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(local)\\sqlexpress;Database=WashersDB;Integrated Security=true;trustservercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Washer>(entity =>
        {
            entity.HasKey(e => e.WasherId).HasName("PK__Washers__7D0229BF146E5FC6");

            entity.Property(e => e.WasherId).HasColumnName("WasherID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.WasherName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
