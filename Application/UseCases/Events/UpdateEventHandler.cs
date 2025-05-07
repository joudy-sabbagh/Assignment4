// Application/UseCases/Events/UpdateEventHandler.cs
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Events
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Unit>
    {
        private readonly IEventRepository _repo;
        private readonly ILogger<UpdateEventHandler> _logger;

        public UpdateEventHandler(IEventRepository repo, ILogger<UpdateEventHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            _logger.LogInformation("Updating event {Id} with {@Dto}", dto.Id, dto);

            var ev = await _repo.GetByIdAsync(dto.Id);
            if (ev == null)
            {
                _logger.LogWarning("Event {Id} not found", dto.Id);
                throw new KeyNotFoundException($"Event with ID {dto.Id} not found.");
            }

            ev.UpdateDetails(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );

            await _repo.UpdateAsync(ev);
            _logger.LogInformation("Updated event {Id}", dto.Id);
            return Unit.Value;
        }
    }
}
