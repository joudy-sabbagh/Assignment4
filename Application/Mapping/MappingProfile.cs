// Application/Mapping/MappingProfile.cs
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Command-side mappings
            CreateMap<CreateAttendeeDTO, Attendee>();
            CreateMap<UpdateAttendeeDTO, Attendee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateEventDTO, Event>();
            CreateMap<UpdateEventDTO, Event>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateTicketDTO, Ticket>();
            CreateMap<UpdateTicketDTO, Ticket>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateVenueDTO, Venue>();
            CreateMap<UpdateVenueDTO, Venue>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Read-side (list) mappings
            CreateMap<Attendee, AttendeeListDTO>();

            CreateMap<Venue, VenueListDTO>();

            CreateMap<Event, EventListDTO>()
                .ForMember(dest => dest.VenueId,
                           opt => opt.MapFrom(src => src.VenueId));

            CreateMap<Ticket, TicketListDTO>()
                .ForMember(dest => dest.Category,
                           opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.EventId,
                           opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.AttendeeId,
                           opt => opt.MapFrom(src => src.AttendeeId));
        }
    }
}
