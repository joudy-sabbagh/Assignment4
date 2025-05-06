using AutoMapper;
using Domain.Entities;
using Application.DTOs;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Attendee mappings
            CreateMap<CreateAttendeeDTO, Attendee>();
            CreateMap<UpdateAttendeeDTO, Attendee>();

            // Event mappings
            CreateMap<CreateEventDTO, Event>();
            CreateMap<UpdateEventDTO, Event>();

            // Ticket mappings
            CreateMap<CreateTicketDTO, Ticket>();
            CreateMap<UpdateTicketDTO, Ticket>();

            // Venue mappings
            CreateMap<CreateVenueDTO, Venue>();
            CreateMap<UpdateVenueDTO, Venue>();
        }
    }
}
