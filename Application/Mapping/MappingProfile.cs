using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Application.DTOs;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Money, decimal>().ConvertUsing(m => m.Amount);
            CreateMap<EmailAddress, string>().ConvertUsing(e => e.Value);

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

            CreateMap<Attendee, AttendeeListDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<Venue, VenueListDTO>();

            CreateMap<Event, EventListDTO>()
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId));

            CreateMap<Ticket, TicketListDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.AttendeeId, opt => opt.MapFrom(src => src.AttendeeId))
                .ForMember(dest => dest.EventName,
                           opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.AttendeeName,
                           opt => opt.MapFrom(src => src.Attendee.Name));

        }
    }
}
