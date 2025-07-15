using HotelBookingAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Data
{
    /// <summary>
    /// Represents the database context for the Hotel Booking API.
    /// Includes configuration for Hotels, Rooms, and Bookings.
    /// </summary>
    public class HotelContext(DbContextOptions<HotelContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the Hotels in the database.
        /// </summary>
        public DbSet<Hotel> Hotels { get; set; }

        /// <summary>
        /// Gets or sets the Rooms in the database.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the Bookings in the database.
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Configures the model using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">Model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------------
            // Hotel Entity Configuration
            // -------------------------------

            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms) // A hotel has many rooms
                .WithOne()             // Rooms are associated with one hotel
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade); // Delete rooms when a hotel is deleted

            modelBuilder.Entity<Hotel>()
                .Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100); // Name is required with max length 100


            // -------------------------------
            // Room Entity Configuration
            // -------------------------------

            modelBuilder.Entity<Room>()
                .HasKey(r => new { r.HotelId, r.Id }); // Composite key: Room ID + Hotel ID

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Bookings); // A room has many bookings


            // -------------------------------
            // Booking Entity Configuration
            // -------------------------------

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique(); // Booking reference must be unique

            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingReference)
                .IsRequired()
                .HasMaxLength(50); // Required field with max length
        }
    }
}
