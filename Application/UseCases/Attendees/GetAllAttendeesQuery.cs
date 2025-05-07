using System.Collections.Generic;
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesQuery : IRequest<List<AttendeeListDTO>> { }
}
