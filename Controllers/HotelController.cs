using HotelBookingAPI.Data;
using HotelBookingAPI.DTO;
using HotelBookingAPI.Model;
using HotelBookingAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController(IServices services, HotelContext context) : ControllerBase
    {
        private readonly IServices _services = services;
        private readonly HotelContext _context = context;

        /// <summary>
        /// Seeds the database with random hotels and rooms.
        /// </summary>
        /// <returns>Status of the seeding operation.</returns>
        [HttpPost("seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Seed()
        {
            if (_context.Hotels.Any())
                return BadRequest("Data already exists.");

            var random = new Random();
            int hotelCount = random.Next(10, 40);

            for (int h = 1; h <= hotelCount; h++)
            {
                int roomId = 1;
                var hotel = new Hotel
                {
                    Name = $"Hotel{h}",
                    Rooms = []
                };

                var roomTypes = Enum.GetValues<RoomType>().Cast<RoomType>().ToList();

                while (hotel.Rooms.Count < 6)
                {
                    if (hotel.Rooms.Count <= 3)
                    {
                        foreach (var room in roomTypes)
                        {
                            hotel.Rooms.Add(new Room
                            {
                                Id = roomId++,
                                Type = room,
                                Capacity = (int)room
                            });
                        }
                    }
                    else
                    {
                        var randomType = (RoomType)roomTypes[random.Next(roomTypes.Count)];
                        hotel.Rooms.Add(new Room
                        {
                            Id = roomId++,
                            Type = randomType,
                            Capacity = (int)randomType
                        });
                    }
                }

                _context.Hotels.Add(hotel);
            }

            _context.SaveChanges();

            return Ok($"Database seeded with {hotelCount} hotels.");
        }

        /// <summary>
        /// Clears all data from the database.
        /// </summary>
        /// <returns>Status of the reset operation.</returns>
        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Reset()
        {
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.Rooms.RemoveRange(_context.Rooms);
            _context.Hotels.RemoveRange(_context.Hotels);
            _context.SaveChanges();
            return Ok("Database reset.");
        }

        /// <summary>
        /// Retrieves all hotels.
        /// </summary>
        /// <returns>List of hotels.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _services.GetAllHotelsAsync();
            return Ok(hotels);
        }

        /// <summary>
        /// Searches for a hotel by name.
        /// </summary>
        /// <param name="name">Name of the hotel.</param>
        /// <returns>Matching hotel or not found response.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(HotelDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelDTO>> GetHotelByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Hotel name is required.");

            var hotel = await _services.GetHotelByNameAsync(name);

            if (hotel == null)
                return NotFound($"Hotel with name '{name}' not found.");

            return Ok(hotel);
        }

        /// <summary>
        /// Gets available rooms in a hotel for a given date range and number of guests.
        /// </summary>
        /// <param name="hotelId">Hotel ID.</param>
        /// <param name="startDate">Start date of stay.</param>
        /// <param name="endDate">End date of stay.</param>
        /// <param name="numberOfGuests">Number of guests.</param>
        /// <returns>List of available rooms.</returns>
        [HttpGet("{hotelId}/available-rooms")]
        [ProducesResponseType(typeof(List<RoomDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RoomDTO>>> GetAvailableRooms(
            int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int numberOfGuests)
        {
            if (numberOfGuests <= 0)
                return BadRequest("Number of guests must be greater than 0.");

            if (startDate >= endDate)
                return BadRequest("Start date must be before end date.");

            var rooms = await _services.GetAvailableRoomsAsync(hotelId, startDate, endDate, numberOfGuests);

            return Ok(rooms);
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="bookingDto">Booking details.</param>
        /// <returns>Created booking.</returns>
        [HttpPost("book")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            try
            {
                var created = await _services.CreateBookingAsync(bookingDto);
                return CreatedAtAction(nameof(GetBookingByReference),
                                       new { reference = created.BookingReference },
                                       created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a booking by its reference code.
        /// </summary>
        /// <param name="reference">Booking reference.</param>
        /// <returns>Booking details or not found.</returns>
        [HttpGet("{reference}")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDTO>> GetBookingByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return BadRequest("Booking reference is required.");

            var booking = await _services.GetBookingByReferenceAsync(reference);

            if (booking == null)
                return NotFound($"Booking with reference '{reference}' not found.");

            return Ok(booking);
        }

        /// <summary>
        /// Gets all rooms for a specific hotel.
        /// </summary>
        /// <param name="hotelName">Hotel name.</param>
        /// <returns>List of rooms.</returns>
        [HttpGet("hotels/{hotelName}/rooms")]
        [ProducesResponseType(typeof(List<RoomDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<RoomDTO>>> GetRoomsByHotelId(string hotelName)
        {
            var hotel = await _services.GetHotelByNameAsync(hotelName);
            if (hotel == null)
                return NotFound($"Hotel with name {hotelName} not found.");

            var rooms = await _services.GetRoomsByHotelIdAsync(hotel.Id);
            return Ok(rooms);
        }
    }
}
