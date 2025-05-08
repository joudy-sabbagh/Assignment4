// Application/UseCases/Events/GetAllEventsQuery.cs
using System.Collections.Generic;
using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Events
{
    public class GetAllEventsQuery : IRequest<Result<List<EventListDTO>>> { }
}
