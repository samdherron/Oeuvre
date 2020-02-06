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

        public virtual DbSet<Gallery> Gallery { get; set; }
        public virtual DbSet<GalleryAuth> GalleryAuth { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Theme> Theme { get; set; }
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
            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.Property(e => e.GalleryId)
                    .HasColumnName("Gallery_ID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GalleryAuthId)
                    .IsRequired()
                    .HasColumnName("Gallery_AuthID")
                    .HasMaxLength(10)
                    .IsFixedLength();

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

                entity.HasOne(d => d.GalleryAuth)
                    .WithMany(p => p.Gallery)
                    .HasForeignKey(d => d.GalleryAuthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gallery_GalleryAuth");
            });

            modelBuilder.Entity<GalleryAuth>(entity =>
            {
                entity.ToTable("Gallery_Auth");

                entity.Property(e => e.GalleryAuthId)
                    .HasColumnName("GalleryAuthID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username).IsRequired();
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.ImgId);

                entity.Property(e => e.ImgId)
                    .HasColumnName("Img_ID")
                    .HasMaxLength(10)
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

                entity.Property(e => e.ThemeId)
                    .IsRequired()
                    .HasColumnName("Theme_ID")
                    .HasMaxLength(10)
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
