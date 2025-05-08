// Application/UseCases/Events/CreateEventHandler.cs
using MediatR;                                // IRequestHandler<,>
using Domain.Interfaces;                      // IEventRepository
using Domain.Services;                        // IEventValidationService
using Microsoft.Extensions.Logging;           // ILogger<>
using Application.UseCases.Events;            // CreateEventCommand
using Domain.Entities;                        // Event

namespace Application.UseCases.Events
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IEventValidationService _validation;
        private readonly ILogger<CreateEventHandler> _logger;

        public CreateEventHandler(
            IEventRepository eventRepo,
            IEventValidationService validation,
            ILogger<CreateEventHandler> logger)
        {
            _eventRepo = eventRepo;
            _validation = validation;
            _logger = logger;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // enforce business rules via domain service
            _validation.ValidateEventDetails(
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice);

            var newEvent = new Event(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );

            await _eventRepo.AddAsync(newEvent);
            _logger.LogInformation("Created Event {EventId}", newEvent.Id);
            return newEvent.Id;
        }
    }
}
