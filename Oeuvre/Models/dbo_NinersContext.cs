using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Oeuvre.Models
{
    public partial class dbo_NinersContext : DbContext
    {
        public dbo_NinersContext()
        {
        }

        public dbo_NinersContext(DbContextOptions<dbo_NinersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<BusinessGames> BusinessGames { get; set; }
        public virtual DbSet<BusinessLogin> BusinessLogin { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<GamingCategory> GamingCategory { get; set; }
        public virtual DbSet<GamingInfo> GamingInfo { get; set; }
        public virtual DbSet<GamingPlatform> GamingPlatform { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberLogin> MemberLogin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dbo_Niners;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.Property(e => e.BusinessId).HasColumnName("businessID");

                entity.Property(e => e.BusinessAddress)
                    .IsRequired()
                    .HasColumnName("businessAddress")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCity)
                    .IsRequired()
                    .HasColumnName("businessCity")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCountry)
                    .IsRequired()
                    .HasColumnName("businessCountry")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessPhoneNumber)
                    .IsRequired()
                    .HasColumnName("businessPhoneNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessPostalCode)
                    .IsRequired()
                    .HasColumnName("businessPostalCode")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BusinessGames>(entity =>
            {
                entity.HasKey(e => e.GameId)
                    .HasName("PK__Business__DA90B4B28F142608");

                entity.Property(e => e.GameId)
                    .HasColumnName("gameID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessId).HasColumnName("businessID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessGames)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessG__busin__32E0915F");

                entity.HasOne(d => d.Game)
                    .WithOne(p => p.BusinessGames)
                    .HasForeignKey<BusinessGames>(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessG__gameI__33D4B598");
            });

            modelBuilder.Entity<BusinessLogin>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Psswd)
                    .IsRequired()
                    .HasColumnName("psswd")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.BusinessLogin)
                    .HasForeignKey<BusinessLogin>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessLogi__ID__34C8D9D1");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.CartId).HasColumnName("cartID");

                entity.Property(e => e.MemberId).HasColumnName("memberID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__memberID__5070F446");
            });

            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.HasKey(e => e.CartItemdId)
                    .HasName("PK__CartItem__F3C2D752AF07666A");

                entity.Property(e => e.CartItemdId).HasColumnName("cartItemdID");

                entity.Property(e => e.CartId).HasColumnName("cartID");

                entity.Property(e => e.GameId).HasColumnName("gameID");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CartItems__cartI__534D60F1");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CartItems__gameI__5441852A");
            });

            modelBuilder.Entity<GamingCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__GamingCa__23CAF1F8057C4005");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("categoryName")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GamingInfo>(entity =>
            {
                entity.HasKey(e => e.GameId)
                    .HasName("PK__GamingIn__DA90B4B2B86CB5EF");

                entity.Property(e => e.GameId).HasColumnName("gameID");

                entity.Property(e => e.GameCategory).HasColumnName("gameCategory");

                entity.Property(e => e.GameDescription)
                    .IsRequired()
                    .HasColumnName("gameDescription")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GamePlatform).HasColumnName("gamePlatform");

                entity.Property(e => e.GamePrice).HasColumnName("gamePrice");

                entity.Property(e => e.GameTitle)
                    .IsRequired()
                    .HasColumnName("gameTitle")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("releaseDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.GameCategoryNavigation)
                    .WithMany(p => p.GamingInfo)
                    .HasForeignKey(d => d.GameCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GamingInf__gameC__37A5467C");

                entity.HasOne(d => d.GamePlatformNavigation)
                    .WithMany(p => p.GamingInfo)
                    .HasForeignKey(d => d.GamePlatform)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GamingInf__gameP__38996AB5");
            });

            modelBuilder.Entity<GamingPlatform>(entity =>
            {
                entity.HasKey(e => e.PlatformId)
                    .HasName("PK__GamingPl__C5540EABFFEB080F");

                entity.Property(e => e.PlatformId).HasColumnName("platformID");

                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasColumnName("platformName")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasColumnName("memberID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberAddress)
                    .IsRequired()
                    .HasColumnName("memberAddress")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberCity)
                    .IsRequired()
                    .HasColumnName("memberCity")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberCountry)
                    .IsRequired()
                    .HasColumnName("memberCountry")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberEmail)
                    .IsRequired()
                    .HasColumnName("memberEmail")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberPhoneNumber)
                    .IsRequired()
                    .HasColumnName("memberPhoneNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MemberPostalCode)
                    .IsRequired()
                    .HasColumnName("memberPostalCode")
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemberLogin>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__MemberLo__7FD7CFF66CC34FB1");

                entity.Property(e => e.MemberId)
                    .HasColumnName("memberID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.Property(e => e.MemberName)
                    .HasColumnName("memberName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberPassword)
                    .IsRequired()
                    .HasColumnName("memberPassword")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberUsername)
                    .IsRequired()
                    .HasColumnName("memberUsername")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithOne(p => p.MemberLogin)
                    .HasForeignKey<MemberLogin>(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberLog__membe__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
