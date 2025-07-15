using HotelBookingAPI.DTO;
using HotelBookingAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingAPI.Service
{
    /// <summary>
    /// Defines service layer operations for hotels, rooms, and bookings.
    /// </summary>
    public interface IServices
    {
        /// <summary>
        /// Retrieves all hotels from the system asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="Hotel"/> entities.</returns>
        Task<List<Hotel>> GetAllHotelsAsync();

        /// <summary>
        /// Finds a hotel by its name asynchronously.
        /// </summary>
        /// <param name="name">The exact name of the hotel to find.</param>
        /// <returns>A <see cref="HotelDTO"/> representing the hotel, or null if not found.</returns>
        Task<HotelDTO> GetHotelByNameAsync(string name);

        /// <summary>
        /// Retrieves available rooms in a specific hotel for a given date range and guest count asynchronously.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="startDate">Start date of the stay.</param>
        /// <param name="endDate">End date of the stay.</param>
        /// <param name="numberOfGuests">Number of guests to accommodate.</param>
        /// <returns>A list of <see cref="RoomDTO"/> objects representing available rooms.</returns>
        Task<List<RoomDTO>> GetAvailableRoomsAsync(int hotelId, DateTime startDate, DateTime endDate, int numberOfGuests);

        /// <summary>
        /// Retrieves a booking by its unique booking reference asynchronously.
        /// </summary>
        /// <param name="reference">The booking reference code.</param>
        /// <returns>A <see cref="BookingDTO"/> representing the booking, or null if not found.</returns>
        Task<BookingDTO> GetBookingByReferenceAsync(string reference);

        /// <summary>
        /// Creates a new booking asynchronously.
        /// </summary>
        /// <param name="bookingDto">The booking data transfer object containing booking details.</param>
        /// <returns>The created <see cref="BookingDTO"/> with assigned reference.</returns>
        Task<BookingDTO> CreateBookingAsync(BookingDTO bookingDto);

        /// <summary>
        /// Retrieves all rooms for a specific hotel asynchronously.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of <see cref="RoomDTO"/> objects for the hotel.</returns>
        Task<List<RoomDTO>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
