using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StartFMS.Models.Backend
{
    public partial class A00_BackendContext : DbContext
    {
        public A00_BackendContext()
        {
        }

        public A00_BackendContext(DbContextOptions<A00_BackendContext> options)
            : base(options)
        {
        }

        public virtual DbSet<A00Account> A00Accounts { get; set; } = null!;
        public virtual DbSet<A00Division> A00Divisions { get; set; } = null!;
        public virtual DbSet<A00JobTitle> A00JobTitles { get; set; } = null!;
        public virtual DbSet<A00Role> A00Roles { get; set; } = null!;
        public virtual DbSet<B10LineMessageOption> B10LineMessageOptions { get; set; } = null!;
        public virtual DbSet<B10LineMessageType> B10LineMessageTypes { get; set; } = null!;
        public virtual DbSet<S01MenuBasicSetting> S01MenuBasicSettings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-2HU7NL0\\SQLEXPRESS;Initial Catalog=StartFMS_Backend;Persist Security Info=True;User ID=sa;Password=root");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A00Account>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__A00_Acco__7AD04F11B573F9A5");

                entity.ToTable("A00_Account");

                entity.Property(e => e.EmployeeId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.A00Accounts)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_ToTable_1");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.A00Accounts)
                    .HasForeignKey(d => d.JobTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_ToTable");
            });

            modelBuilder.Entity<A00Division>(entity =>
            {
                entity.HasKey(e => e.DivisionId)
                    .HasName("PK__A00_Divi__20EFC6A8CCB5E78C");

                entity.ToTable("A00_Division");

                entity.Property(e => e.DivisionId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<A00JobTitle>(entity =>
            {
                entity.HasKey(e => e.JobTitleId)
                    .HasName("PK__A00_JobT__35382FE99AC0583C");

                entity.ToTable("A00_JobTitle");

                entity.Property(e => e.JobTitleId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<A00Role>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__A00_Role__8AFACE1A3E7C0A0F");

                entity.ToTable("A00_Role");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<B10LineMessageOption>(entity =>
            {
                entity.ToTable("B10_LineMessageOption");

                entity.Property(e => e.IsUse)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'false')");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserId)
                    .HasMaxLength(400)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<B10LineMessageType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("B10_LineMessageType");

                entity.Property(e => e.TypeId).HasMaxLength(50);

                entity.Property(e => e.TypeMemo)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<S01MenuBasicSetting>(entity =>
            {
                entity.ToTable("S01_MenuBasicSetting");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DisplayOrder).HasComment("顯示順序 (透過Id抓取，判斷在第幾層位置)");

                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("畫面Icon");

                entity.Property(e => e.MenuName).HasMaxLength(30);

                entity.Property(e => e.ParentId).HasComment("父層ID (目前設為 Id)");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
