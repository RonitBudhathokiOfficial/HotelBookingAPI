using System.Text.Json.Serialization;

namespace HotelBookingAPI.DTO
{
    /// <summary>
    /// Represents basic hotel data for API responses.
    /// </summary>
    public class HotelDTO
    {
        /// <summary>
        /// Unique identifier for the hotel.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the hotel.
        /// </summary>
        public string Name { get; set; } = null!;
    }

    /// <summary>
    /// Represents room information exposed via the API.
    /// </summary>
    public class RoomDTO
    {
        /// <summary>
        /// Unique room identifier (within a hotel).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type of the room (e.g., Single, Double, Deluxe).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Maximum capacity of the room (number of guests it can hold).
        /// </summary>
        public int Capacity { get; set; }
    }

    /// <summary>
    /// Represents a booking to be created or retrieved.
    /// </summary>
    public class BookingDTO
    {
        /// <summary>
        /// Unique booking reference code.
        /// </summary>
        public string BookingReference { get; set; } = null!;

        /// <summary>
        /// ID of the booked room.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// ID of the hotel where the booking is made.
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Number of guests for the booking.
        /// </summary>
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Check-in date of the booking.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Check-out date of the booking.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
