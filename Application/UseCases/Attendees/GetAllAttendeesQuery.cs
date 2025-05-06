using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesQuery : IRequest<List<Attendee>> { }
}
