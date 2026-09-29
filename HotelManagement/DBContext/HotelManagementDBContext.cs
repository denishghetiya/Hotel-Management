using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DBContext;

public partial class HotelManagementDBContext : DbContext
{
    public HotelManagementDBContext(DbContextOptions<HotelManagementDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemList> ItemLists { get; set; }

    public virtual DbSet<OrderItemList> OrderItemLists { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    public virtual DbSet<TableList> TableLists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemList>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.ToTable("ItemList");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemListCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_ItemList_CreatedBy_Users_UserId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ItemListUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ItemList_UpdatedBy_Users_UserId");
        });

        modelBuilder.Entity<OrderItemList>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.ToTable("OrderItemList");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderItemListCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_OrderItemList_CreatedBy_Users_UserId");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItemLists)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItemList_ItemId_ItemList_ItemId");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItemLists)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItemList_OrderId_OrderList_OrderId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OrderItemListUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_OrderItemList_UpdatedBy_Users_UserId");
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("OrderList");

            entity.Property(e => e.BillAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderListCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_OrderList_CreatedBy_Users_UserId");

            entity.HasOne(d => d.Table).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderList_TableId_TableList_TableId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OrderListUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_OrderList_UpdatedBy_Users_UserId");
        });

        modelBuilder.Entity<TableList>(entity =>
        {
            entity.HasKey(e => e.TableId);

            entity.ToTable("TableList");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TableListCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_TableList_CreatedBy_Users_UserId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TableListUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_TableList_UpdatedBy_Users_UserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ResetTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Users_CreatedBy_Users_UserId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Users_UpdatedBy_Users_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
