using Core.Interfaces;

namespace Application.UseCases.Events;

public class DeleteEventHandler
{
    private readonly IEventRepository _eventRepo;

    public DeleteEventHandler(IEventRepository eventRepo)
    {
        _eventRepo = eventRepo;
    }

    public async Task Handle(int id)
    {
        await _eventRepo.DeleteAsync(id);
    }
}
