// Application/UseCases/Venues/GetAllVenuesHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesHandler
        : IRequestHandler<GetAllVenuesQuery, List<VenueListDTO>>
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

        public async Task<List<VenueListDTO>> Handle(GetAllVenuesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllVenuesQuery");

            var entities = await _repo.GetAllAsync();
            var dtos = _mapper.Map<List<VenueListDTO>>(entities);

            _logger.LogInformation("Retrieved {Count} venues", dtos.Count);
            return dtos;
        }
    }
}
