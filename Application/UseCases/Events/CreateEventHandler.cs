// Application/UseCases/Events/CreateEventHandler.cs
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Events
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ILogger<CreateEventHandler> _logger;

        public CreateEventHandler(IEventRepository eventRepo, ILogger<CreateEventHandler> logger)
        {
            _eventRepo = eventRepo;
            _logger = logger;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating event {@Dto}", request.Dto);

            var dto = request.Dto;
            var newEvent = new Event(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );

            await _eventRepo.AddAsync(newEvent);
            _logger.LogInformation("Created Event with Id {EventId}", newEvent.Id);
            return newEvent.Id;
        }
    }
}
