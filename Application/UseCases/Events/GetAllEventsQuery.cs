using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Events
{
    public class GetAllEventsQuery : IRequest<List<Event>> { }
}
