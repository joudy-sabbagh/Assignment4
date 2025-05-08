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

namespace Application.UseCases.Venues
{
    public class GetAllVenuesHandler
        : IRequestHandler<GetAllVenuesQuery, Result<List<VenueListDTO>>>
    {
        private readonly IVenueRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllVenuesHandler> _logger;

        public GetAllVenuesHandler(
            IVenueRepository repo,
            IMapper mapper,
            ILogger<GetAllVenuesHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<VenueListDTO>>> Handle(
            GetAllVenuesQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllVenuesQuery");

            var entities = await _repo.GetAllAsync();
            if (entities == null || !entities.Any())
            {
                _logger.LogWarning("No venues found");
                return Result<List<VenueListDTO>>.Failure("No venues available");
            }

            var dtos = _mapper.Map<List<VenueListDTO>>(entities);
            _logger.LogInformation("Retrieved {Count} venues", dtos.Count);
            return Result<List<VenueListDTO>>.Success(dtos);
        }
    }
}
