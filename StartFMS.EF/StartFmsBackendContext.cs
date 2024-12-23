﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StartFMS.EF;

public partial class StartFmsBackendContext : DbContext
{
    public StartFmsBackendContext()
    {
    }

    public StartFmsBackendContext(DbContextOptions<StartFmsBackendContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SystemCatalogItem> SystemCatalogItems { get; set; }

    public virtual DbSet<SystemParameter> SystemParameters { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=FLOWZ13-NB-H7V4;Database=SFMBS;Trusted_Connection=False;user id=sa;password=root;Encrypt=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemCatalogItem>(entity =>
        {
            entity.Property(e => e.Id)
                .HasComment("目錄識別碼")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasComment("備註");
            entity.Property(e => e.DisplayOrder).HasComment("顯示順序 (透過Id抓取，判斷在第幾層位置)");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasComment("Icon");
            entity.Property(e => e.ImportAt).HasMaxLength(255);
            entity.Property(e => e.IsGroup).HasDefaultValue(false);
            entity.Property(e => e.MenuName)
                .HasMaxLength(30)
                .HasComment("目錄名稱");
            entity.Property(e => e.ParentId).HasComment("父層ID (目前設為 Id)");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasComment("網址..");
        });

        modelBuilder.Entity<SystemParameter>(entity =>
        {
            entity.ToTable("SystemParameter");

            entity.Property(e => e.Id).HasComment("識別碼");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("名稱");
            entity.Property(e => e.Value1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("參數1");
            entity.Property(e => e.Value2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("參數2");
            entity.Property(e => e.Value3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("參數3");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasComment("識別碼")
                .HasColumnName("ID");
            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .HasComment("帳號");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasComment("是否啟用");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasComment("密碼");
            entity.Property(e => e.UserRoleId)
                .HasComment("使用者角色ID (UserRole)")
                .HasColumnName("UserRoleID");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasComment("識別碼")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasComment("備註");
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("名稱")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
