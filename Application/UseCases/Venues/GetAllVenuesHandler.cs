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
            return await _venueRepo.GetAllAsync();
        }
    }
}
