using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _21._02hw.Models;

public partial class FirmKancTovarovContext : DbContext
{
    public FirmKancTovarovContext()
    {
    }

    public FirmKancTovarovContext(DbContextOptions<FirmKancTovarovContext> options)
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
            entity.HasKey(e => e.FirmId).HasName("PK__Firms__1F1F20FC7569FC76");

            entity.Property(e => e.FirmId).HasColumnName("FirmID");
            entity.Property(e => e.FirmName).HasMaxLength(255);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__Managers__3BA2AA81981DDB3E");

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.FullName).HasMaxLength(255);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C41FAD5EA4CD");

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.FirmId).HasColumnName("FirmID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Firm).WithMany(p => p.Sales)
                .HasForeignKey(d => d.FirmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_FirmID");

            entity.HasOne(d => d.Manager).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_ManagerID");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PK__SaleDeta__70DB141E29066806");

            entity.Property(e => e.SaleDetailId).HasColumnName("SaleDetailID");
            entity.Property(e => e.PriceEach).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.StationeryId).HasColumnName("StationeryID");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_SaleDetails_SaleID");

            entity.HasOne(d => d.Stationery).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.StationeryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetails_StationeryID");
        });

        modelBuilder.Entity<Stationery>(entity =>
        {
            entity.HasKey(e => e.StationeryId).HasName("PK__Statione__53026DE3243EF3D2");

            entity.ToTable("Stationery");

            entity.Property(e => e.StationeryId).HasColumnName("StationeryID");
            entity.Property(e => e.CostPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Stationeries)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Stationery_TypeID");
        });

        modelBuilder.Entity<StationeryType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Statione__516F039561CD5ED2");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
