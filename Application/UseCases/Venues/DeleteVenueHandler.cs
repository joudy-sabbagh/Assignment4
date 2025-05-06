using Core.Interfaces;

namespace Application.UseCases.Venues;

public class DeleteVenueHandler
{
    private readonly IVenueRepository _venueRepo;

    public DeleteVenueHandler(IVenueRepository venueRepo)
    {
        _venueRepo = venueRepo;
    }

    public async Task Handle(int id)
    {
        await _venueRepo.DeleteAsync(id);
    }
}
