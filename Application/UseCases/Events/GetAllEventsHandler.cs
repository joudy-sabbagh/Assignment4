using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.Common;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Events
{
    public class GetAllEventsHandler
        : IRequestHandler<GetAllEventsQuery, Result<List<EventListDTO>>>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllEventsHandler> _logger;

        public GetAllEventsHandler(
            IEventRepository repo,
            IMapper mapper,
            ILogger<GetAllEventsHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<EventListDTO>>> Handle(
            GetAllEventsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllEventsQuery");
            var entities = await _repo.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                _logger.LogWarning("No events found");
                return Result<List<EventListDTO>>.Failure("No events available");
            }

            var dtos = _mapper.Map<List<EventListDTO>>(entities);
            _logger.LogInformation("Retrieved {Count} events", dtos.Count);
            return Result<List<EventListDTO>>.Success(dtos);
        }
    }
}
