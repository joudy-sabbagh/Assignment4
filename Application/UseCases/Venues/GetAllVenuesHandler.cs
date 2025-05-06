using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Venues;

public class GetAllVenuesHandler
{
    private readonly IVenueRepository _venueRepo;

    public GetAllVenuesHandler(IVenueRepository venueRepo)
    {
        _venueRepo = venueRepo;
    }

    public async Task<List<Venue>> Handle()
    {
        return await _venueRepo.GetAllAsync();
    }
}
