using AutoMapper;
using HotelBookingAPI.Data;
using HotelBookingAPI.DTO;
using HotelBookingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Service
{
    /// <summary>
    /// Implementation of <see cref="IServices"/> providing business logic for hotels, rooms, and bookings.
    /// </summary>
    public class Services(HotelContext context, IMapper mapper) : IServices
    {
        private readonly HotelContext _context = context;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retrieves all hotels including their rooms asynchronously.
        /// </summary>
        /// <returns>List of all <see cref="Hotel"/> entities.</returns>
        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a hotel by name (case-insensitive).
        /// </summary>
        /// <param name="name">Hotel name to search for.</param>
        /// <returns>A <see cref="HotelDTO"/> if found; otherwise null.</returns>
        public async Task<HotelDTO> GetHotelByNameAsync(string name)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Name.ToLower()
                    .Equals(name.ToLower(), StringComparison.CurrentCultureIgnoreCase));

            return hotel == null ? null : _mapper.Map<HotelDTO>(hotel);
        }

        /// <summary>
        /// Retrieves rooms available for booking within the specified date range and guest count.
        /// </summary>
        /// <param name="hotelId">Hotel ID.</param>
        /// <param name="startDate">Start date of the stay.</param>
        /// <param name="endDate">End date of the stay.</param>
        /// <param name="numberOfGuests">Number of guests to accommodate.</param>
        /// <returns>List of available <see cref="RoomDTO"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if endDate is not after startDate.</exception>
        public async Task<List<RoomDTO>> GetAvailableRoomsAsync(int hotelId, DateTime startDate, DateTime endDate, int numberOfGuests)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;

            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Capacity >= numberOfGuests)
                .Include(r => r.Bookings)
                .ToListAsync();

            // Only select rooms without overlapping bookings for the requested date range
            var availableRooms = rooms
                .Where(room => room.Bookings.All(booking =>
                    booking.EndDate <= startDate || booking.StartDate >= endDate))
                .ToList();

            return _mapper.Map<List<RoomDTO>>(availableRooms);
        }

        /// <summary>
        /// Finds a booking by its unique reference code.
        /// </summary>
        /// <param name="reference">Booking reference string.</param>
        /// <returns>A <see cref="BookingDTO"/> if found; otherwise null.</returns>
        public async Task<BookingDTO> GetBookingByReferenceAsync(string reference)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingReference == reference);

            return booking == null ? null : _mapper.Map<BookingDTO>(booking);
        }

        /// <summary>
        /// Creates a new booking if room is available and number of guests fits capacity.
        /// </summary>
        /// <param name="bookingDto">Booking details.</param>
        /// <returns>The created <see cref="BookingDTO"/> with assigned reference.</returns>
        /// <exception cref="ArgumentException">If end date is before or equal to start date.</exception>
        /// <exception cref="InvalidOperationException">If room not found, guest number exceeds capacity, or dates overlap existing bookings.</exception>
        public async Task<BookingDTO> CreateBookingAsync(BookingDTO bookingDto)
        {
            // Normalize dates to date-only
            var startDate = bookingDto.StartDate.Date;
            var endDate = bookingDto.EndDate.Date;

            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == bookingDto.RoomId)
                ?? throw new InvalidOperationException("Room not found.");

            if (bookingDto.NumberOfGuests > room.Capacity)
                throw new InvalidOperationException("Number of guests exceeds room capacity.");

            // Check for any overlapping bookings
            var isOverlapping = room.Bookings.Any(b =>
                b.StartDate < endDate && b.EndDate > startDate);

            if (isOverlapping)
                throw new InvalidOperationException("Room is already booked for the given dates.");

            // Generate unique booking reference (8 uppercase chars)
            var bookingReference = Guid.NewGuid().ToString("N")[..8].ToUpper();

            var booking = new Booking
            {
                RoomId = bookingDto.RoomId,
                NumberOfGuests = bookingDto.NumberOfGuests,
                StartDate = startDate,
                EndDate = endDate,
                BookingReference = bookingReference
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDTO>(booking);
        }

        /// <summary>
        /// Retrieves all rooms belonging to a specific hotel.
        /// </summary>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>List of <see cref="RoomDTO"/>.</returns>
        public async Task<List<RoomDTO>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();

            return _mapper.Map<List<RoomDTO>>(rooms);
        }
    }
}
