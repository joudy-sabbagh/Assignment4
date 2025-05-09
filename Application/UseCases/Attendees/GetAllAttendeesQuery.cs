using System.Collections.Generic;
using Application.Common;            
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesQuery : IRequest<Result<List<AttendeeListDTO>>> { }
}
