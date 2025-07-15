using System.Text.Json.Serialization;

namespace HotelBookingAPI.Model
{
    /// <summary>
    /// Defines the types of rooms available in a hotel.
    /// </summary>
    public enum RoomType
    {
        /// <summary>Single room type with capacity 1.</summary>
        Single = 1,

        /// <summary>Double room type with capacity 2.</summary>
        Double = 2,

        /// <summary>Deluxe room type with capacity 3.</summary>
        Deluxe = 3
    }

    /// <summary>
    /// Represents a hotel with a collection of rooms and bookings.
    /// </summary>
    public class Hotel
    {
        /// <summary>
        /// Primary key - unique identifier of the hotel.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the hotel.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Rooms that belong to this hotel.
        /// </summary>
        public ICollection<Room> Rooms { get; set; } = [];

        /// <summary>
        /// Bookings associated with this hotel.
        /// Marked with [JsonIgnore] to avoid circular references during serialization.
        /// </summary>
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = [];
    }

    /// <summary>
    /// Represents a room within a hotel.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room identifier (unique within a hotel).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the hotel to which this room belongs.
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Type of the room (Single, Double, Deluxe).
        /// </summary>
        public RoomType Type { get; set; }

        /// <summary>
        /// Capacity indicating maximum number of guests allowed.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Bookings associated with this room.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = [];
    }

    /// <summary>
    /// Represents a booking made for a hotel room.
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Primary key - unique identifier for the booking.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique reference code for the booking.
        /// </summary>
        public string BookingReference { get; set; } = null!;

        /// <summary>
        /// ID of the room that is booked.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// ID of the hotel for the booking.
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Number of guests included in the booking.
        /// </summary>
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Start date of the booking.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the booking.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
