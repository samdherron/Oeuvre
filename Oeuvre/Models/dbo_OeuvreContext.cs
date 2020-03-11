using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Oeuvre.Models
{
    public partial class dbo_OeuvreContext : DbContext
    {
        public dbo_OeuvreContext()
        {
        }

        public dbo_OeuvreContext(DbContextOptions<dbo_OeuvreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Gallery> Gallery { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<ImgThemes> ImgThemes { get; set; }
        public virtual DbSet<Theme> Theme { get; set; }
        public virtual DbSet<ThemeLookup> ThemeLookup { get; set; }
        public virtual DbSet<ThemeType> ThemeType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dbo_Oeuvre;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.Property(e => e.GalleryId)
                    .HasColumnName("Gallery_ID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AuthUserId).HasMaxLength(450);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GalleryDescription).HasColumnName("Gallery_Description");

                entity.Property(e => e.GalleryName)
                    .IsRequired()
                    .HasColumnName("Gallery_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("Postal_Code")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.ImgId);

                entity.Property(e => e.ImgId)
                    .HasColumnName("Img_ID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Artist).IsUnicode(false);

                entity.Property(e => e.CollectionType).HasColumnName("collectionType");

                entity.Property(e => e.CuratorName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DateUploaded)
                    .HasColumnName("Date_Uploaded")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.GalleryId)
                    .IsRequired()
                    .HasColumnName("Gallery_ID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ImgLocation).HasColumnName("Img_Location");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PieceDimensions)
                    .HasColumnName("pieceDimensions")
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.ThemeId)
                    .IsRequired()
                    .HasColumnName("Theme_ID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.YearCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.GalleryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Gallery");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Theme");
            });

            modelBuilder.Entity<ImgThemes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ThemeId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.ImgThemes)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImgThemes_Theme");

                entity.HasOne(d => d.ThemeLookup)
                    .WithMany(p => p.ImgThemes)
                    .HasForeignKey(d => d.ThemeLookupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImgThemes_ThemeLookup");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.Property(e => e.ThemeId)
                    .HasColumnName("ThemeID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ThemeName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsFixedLength();

                entity.Property(e => e.ThemeTypeId)
                    .IsRequired()
                    .HasColumnName("ThemeTypeID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.ThemeType)
                    .WithMany(p => p.Theme)
                    .HasForeignKey(d => d.ThemeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theme_ThemeType");
            });

            modelBuilder.Entity<ThemeLookup>(entity =>
            {
                entity.Property(e => e.ThemeLookupId).HasColumnName("ThemeLookupID");

                entity.Property(e => e.ImgId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Img)
                    .WithMany(p => p.ThemeLookup)
                    .HasForeignKey(d => d.ImgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThemeLookup_Image");
            });

            modelBuilder.Entity<ThemeType>(entity =>
            {
                entity.Property(e => e.ThemeTypeId)
                    .HasColumnName("ThemeTypeID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ThemeTypeName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
