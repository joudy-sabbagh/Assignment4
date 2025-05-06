using Application.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Events;

public class CreateEventHandler
{
    private readonly IEventRepository _eventRepo;

    public CreateEventHandler(IEventRepository eventRepo)
    {
        _eventRepo = eventRepo;
    }

    public async Task Handle(CreateEventDTO dto)
    {
        var newEvent = new Event
        {
            Name = dto.Name,
            Date = dto.Date,
            NormalPrice = dto.NormalPrice,
            VipPrice = dto.VipPrice,
            BackstagePrice = dto.BackstagePrice,
            VenueId = dto.VenueId
        };

        await _eventRepo.AddAsync(newEvent);
    }
}
