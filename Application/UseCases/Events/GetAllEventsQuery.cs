// Application/UseCases/Events/GetAllEventsQuery.cs
using System.Collections.Generic;
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Events
{
    // unchanged: returns a list of DTOs
    public class GetAllEventsQuery : IRequest<List<EventListDTO>> { }
}
