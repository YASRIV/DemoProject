using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Car_Wash_Library.Models;

public partial class OrderDbContext : DbContext
{
    public OrderDbContext()
    {
    }

    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }

    public virtual DbSet<WashRequest> WashRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local)\\sqlexpress;Database=OrderDB;Integrated Security=true;trustservercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF9D2CB3CA");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.WashDate).HasColumnType("datetime");
            entity.Property(e => e.WasherId).HasDefaultValueSql("(NULL)");
        });

        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasKey(e => e.ServiceOrderId).HasName("PK__ServiceO__8E1ABD6575D1347D");
        });

        modelBuilder.Entity<WashRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__WashRequ__33A8517A1703332C");

            entity.Property(e => e.RequestStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
