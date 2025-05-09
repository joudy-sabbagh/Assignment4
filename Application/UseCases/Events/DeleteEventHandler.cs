using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Events
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Unit>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ILogger<DeleteEventHandler> _logger;

        public DeleteEventHandler(IEventRepository eventRepo, ILogger<DeleteEventHandler> logger)
        {
            _eventRepo = eventRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting event {Id}", request.Id);
            await _eventRepo.DeleteAsync(request.Id);
            _logger.LogInformation("Deleted event {Id}", request.Id);
            return Unit.Value;
        }
    }
}
