using System.Collections.Generic;      // for List<T>
using System.Linq;                     // for .ToList()
using System.Threading;                // for CancellationToken
using System.Threading.Tasks;          // for Task<T>
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesHandler : IRequestHandler<GetAllVenuesQuery, List<Venue>>
    {
        private readonly IVenueRepository _venueRepo;

        public GetAllVenuesHandler(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public async Task<List<Venue>> Handle(GetAllVenuesQuery request, CancellationToken cancellationToken)
        {
            return (await _venueRepo.GetAllAsync()).ToList();
        }
    }
}
