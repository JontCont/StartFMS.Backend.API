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

        public virtual DbSet<A00AccountUser> A00AccountUsers { get; set; } = null!;
        public virtual DbSet<A00AccountUserClaim> A00AccountUserClaims { get; set; } = null!;
        public virtual DbSet<A00AccountUserLogin> A00AccountUserLogins { get; set; } = null!;
        public virtual DbSet<A00AccountUserRole> A00AccountUserRoles { get; set; } = null!;
        public virtual DbSet<A00AccountUserToken> A00AccountUserTokens { get; set; } = null!;
        public virtual DbSet<A01AccountRole> A01AccountRoles { get; set; } = null!;
        public virtual DbSet<A01AccountRoleClaim> A01AccountRoleClaims { get; set; } = null!;
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
            modelBuilder.Entity<A00AccountUser>(entity =>
            {
                entity.ToTable("A00_AccountUsers");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.IsUse).HasMaxLength(20);

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Rfid)
                    .HasMaxLength(20)
                    .HasColumnName("RFID");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<A00AccountUserClaim>(entity =>
            {
                entity.ToTable("A00_AccountUserClaims");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.A00AccountUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<A00AccountUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("A00_AccountUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.A00AccountUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<A00AccountUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AccountUserRoles");

                entity.ToTable("A00_AccountUserRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.A00AccountUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AccountUserRoles_AccountRoles_RoleId");
            });

            modelBuilder.Entity<A00AccountUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.ToTable("A00_AccountUserTokens");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<A01AccountRole>(entity =>
            {
                entity.ToTable("A01_AccountRoles");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<A01AccountRoleClaim>(entity =>
            {
                entity.ToTable("A01_AccountRoleClaims");

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.A01AccountRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AccountRoleClaims_AccountRoles_RoleId");
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
