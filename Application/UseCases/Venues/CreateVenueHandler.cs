using Application.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Venues;

public class CreateVenueHandler
{
    private readonly IVenueRepository _venueRepo;

    public CreateVenueHandler(IVenueRepository venueRepo)
    {
        _venueRepo = venueRepo;
    }

    public async Task Handle(CreateVenueDTO dto)
    {
        var venue = new Venue
        {
            Name = dto.Name,
            Capacity = dto.Capacity
        };

        await _venueRepo.AddAsync(venue);
    }
}
