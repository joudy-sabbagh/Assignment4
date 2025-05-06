using Application.DTOs;
using Core.Interfaces;

namespace Application.UseCases.Venues;

public class EditVenueHandler
{
    private readonly IVenueRepository _venueRepo;

    public EditVenueHandler(IVenueRepository venueRepo)
    {
        _venueRepo = venueRepo;
    }

    public async Task Handle(UpdateVenueDTO dto)
    {
        var venue = await _venueRepo.GetByIdAsync(dto.VenueId);
        if (venue == null) throw new Exception("Venue not found");

        venue.Name = dto.Name;
        venue.Capacity = dto.Capacity;

        await _venueRepo.UpdateAsync(venue);
    }
}
