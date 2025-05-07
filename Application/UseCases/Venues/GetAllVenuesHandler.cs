// Application/UseCases/Venues/GetAllVenuesHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesHandler
        : IRequestHandler<GetAllVenuesQuery, List<VenueListDTO>>
    {
        private readonly IVenueRepository _repo;
        private readonly IMapper _mapper;

        public GetAllVenuesHandler(
            IVenueRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<VenueListDTO>> Handle(
            GetAllVenuesQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<VenueListDTO>>(entities);
        }
    }
}
