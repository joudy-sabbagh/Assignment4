using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Events;

public class GetAllEventsHandler
{
    private readonly IEventRepository _eventRepo;

    public GetAllEventsHandler(IEventRepository eventRepo)
    {
        _eventRepo = eventRepo;
    }

    public async Task<List<Event>> Handle()
    {
        return await _eventRepo.GetAllAsync();
    }
}
