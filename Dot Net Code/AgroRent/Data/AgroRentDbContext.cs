using AgroRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroRent.Data
{
    public class AgroRentDbContext : DbContext
    {
        public AgroRentDbContext(DbContextOptions<AgroRentDbContext> options) : base(options)
        {
        }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Farmer configuration
            modelBuilder.Entity<Farmer>(entity =>
            {
                entity.ToTable("farmers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).HasMaxLength(20).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasMaxLength(30).HasColumnName("last_name");
                entity.Property(e => e.Email).HasMaxLength(30).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Role).HasConversion<string>();
                entity.Property(e => e.Active).HasDefaultValue(true);
                entity.Property(e => e.RazorpayContactId).HasColumnName("razorpay_contact_id");
                entity.Property(e => e.RazorpayFundAccountId).HasColumnName("razorpay_fund_account_id");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Equipment configuration
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(30).HasColumnName("equipment_name");
                entity.Property(e => e.Description);
                entity.Property(e => e.ImageUrl);
                entity.Property(e => e.RentalPrice).IsRequired();
                entity.Property(e => e.Available).HasDefaultValue(true);
                entity.Property(e => e.CloudinaryPublicId);
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
                entity.HasOne(e => e.Owner)
                      .WithMany(f => f.EquipmentList)
                      .HasForeignKey("owner_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDate);
                entity.Property(e => e.EndDate);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.TotalAmount).IsRequired();
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
                entity.HasOne(e => e.Equipment)
                      .WithMany(eq => eq.Bookings)
                      .HasForeignKey("equipment_id")
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Farmer)
                      .WithMany(f => f.Bookings)
                      .HasForeignKey("renter_farmer_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Payment configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PaymentId);
                entity.Property(e => e.OrderId);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Timestamp);
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
                entity.HasOne(e => e.Booking)
                      .WithOne(b => b.Payment)
                      .HasForeignKey<Payment>("booking_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // VerificationToken configuration
            modelBuilder.Entity<VerificationToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token);
                entity.Property(e => e.ExpiryDate);
                entity.HasOne(e => e.Farmer)
                      .WithMany()
                      .HasForeignKey("farmer_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
