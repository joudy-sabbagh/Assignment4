using Application.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Events;

public class EditEventHandler
{
    private readonly IEventRepository _eventRepo;

    public EditEventHandler(IEventRepository eventRepo)
    {
        _eventRepo = eventRepo;
    }

    public async Task Handle(UpdateEventDTO dto)
    {
        var ev = await _eventRepo.GetByIdAsync(dto.EventId);
        if (ev == null) throw new Exception("Event not found");

        ev.Name = dto.Name;
        ev.EventDate = dto.Date;
        ev.NormalPrice = dto.NormalPrice;
        ev.VIPPrice = dto.VipPrice;
        ev.BackstagePrice = dto.BackstagePrice;
        ev.VenueId = dto.VenueId;

        await _eventRepo.UpdateAsync(ev);
    }
}
