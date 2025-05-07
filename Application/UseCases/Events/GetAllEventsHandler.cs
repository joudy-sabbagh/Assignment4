// Application/UseCases/Events/GetAllEventsHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Events
{
    public class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, List<EventListDTO>>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllEventsHandler> _logger;

        public GetAllEventsHandler(IEventRepository repo, IMapper mapper, ILogger<GetAllEventsHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<EventListDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllEventsQuery");
            var entities = await _repo.GetAllAsync();
            var dtos = _mapper.Map<List<EventListDTO>>(entities);
            _logger.LogInformation("Retrieved {Count} events", dtos.Count);
            return dtos;
        }
    }
}
