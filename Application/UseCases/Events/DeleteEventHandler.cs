using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Events
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IEventRepository _eventRepo;

        public DeleteEventHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            await _eventRepo.DeleteAsync(request.Id);
        }
    }
}
