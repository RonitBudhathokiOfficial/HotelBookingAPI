using AutoMapper;
using HotelBookingAPI.DTO;
using HotelBookingAPI.Model;

namespace HotelBookingAPI.Mapping
{
    /// <summary>
    /// Defines AutoMapper mapping profiles between domain models and DTOs.
    /// </summary>
    public class DtoMapper : Profile
    {
        /// <summary>
        /// Configures object-to-object mappings used in the application.
        /// </summary>
        public DtoMapper()
        {
            // Hotel <-> HotelDTO
            // Straightforward mapping with matching property names.
            CreateMap<Hotel, HotelDTO>().ReverseMap();

            // Room <-> RoomDTO
            // Convert RoomType enum to string in DTO, and parse string back to enum when mapping to model.
            CreateMap<Room, RoomDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.Type)));

            // Booking <-> BookingDTO
            // Automatically maps all matching fields (e.g., BookingReference, RoomId, etc.).
            CreateMap<Booking, BookingDTO>().ReverseMap();
        }
    }
}
