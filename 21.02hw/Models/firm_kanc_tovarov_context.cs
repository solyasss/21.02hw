using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _21._02hw.Models;

public partial class firm_kanc_tovarov_context : DbContext
{
    public firm_kanc_tovarov_context()
    {
    }

    public firm_kanc_tovarov_context(DbContextOptions<firm_kanc_tovarov_context> options)
        : base(options)
    {
    }

    public virtual DbSet<Firm> Firms { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<Stationery> Stationeries { get; set; }

    public virtual DbSet<StationeryType> StationeryTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-IB673J5\\SQLEXPRESS;Database=FirmKancTovarov;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Firm>(entity =>
        {
            entity.HasKey(e => e.FirmID).HasName("PK__Firms__1F1F20FC85B9FF5F");

            entity.Property(e => e.FirmName).HasMaxLength(255);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerID).HasName("PK__Managers__3BA2AA8153957BED");

            entity.Property(e => e.FullName).HasMaxLength(255);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleID).HasName("PK__Sales__1EE3C41F710FCBD7");

            entity.Property(e => e.SaleDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Firm).WithMany(p => p.Sales)
                .HasForeignKey(d => d.FirmID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_FirmID");

            entity.HasOne(d => d.Manager).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ManagerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_ManagerID");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailID).HasName("PK__SaleDeta__70DB141E95CE40A4");

            entity.Property(e => e.PriceEach).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleID)
                .HasConstraintName("FK_SaleDetails_SaleID");

            entity.HasOne(d => d.Stationery).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.StationeryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetails_StationeryID");
        });

        modelBuilder.Entity<Stationery>(entity =>
        {
            entity.HasKey(e => e.StationeryID).HasName("PK__Statione__53026DE31A912B11");

            entity.ToTable("Stationery");

            entity.Property(e => e.CostPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Type).WithMany(p => p.Stationeries)
                .HasForeignKey(d => d.TypeID)
                .HasConstraintName("FK_Stationery_TypeID");
        });

        modelBuilder.Entity<StationeryType>(entity =>
        {
            entity.HasKey(e => e.TypeID).HasName("PK__Statione__516F0395787F69B0");

            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
