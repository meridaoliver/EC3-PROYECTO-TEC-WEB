using AutoMapper;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReservationCreateDto, Reservation>();
            CreateMap<Reservation, ReservationDto>()
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ForMember(d => d.SpaceName, o => o.MapFrom(s => s.Space != null ? s.Space.Name : null))
                .ForMember(d => d.Status, o => o.MapFrom(s => (int)s.Status));
            CreateMap<User, CoworkingReservations.Core.DTOs.UserDto>();
            CreateMap<Space, CoworkingReservations.Core.DTOs.SpaceDto>();
        }
    }
}
