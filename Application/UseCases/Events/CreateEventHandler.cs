using MediatR;                                 
using Domain.Interfaces;                       
using Domain.Services;                         
using Microsoft.Extensions.Logging;        
using Application.UseCases.Events;             
using Domain.Entities;                       

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
