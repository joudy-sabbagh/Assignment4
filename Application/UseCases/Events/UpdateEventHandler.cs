using MediatR;                             
using Domain.Interfaces;                    
using Domain.Services;                       
using Microsoft.Extensions.Logging;           
using Application.UseCases.Events;         
using Domain.Entities;                        

namespace Application.UseCases.Events
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Unit>
    {
        private readonly IEventRepository _repo;
        private readonly IEventValidationService _validation;
        private readonly ILogger<UpdateEventHandler> _logger;

        public UpdateEventHandler(
            IEventRepository repo,
            IEventValidationService validation,
            ILogger<UpdateEventHandler> logger)
        {
            _repo = repo;
            _validation = validation;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            _logger.LogInformation("Updating Event {Id}", dto.Id);

            var ev = await _repo.GetByIdAsync(dto.Id);
            if (ev == null)
            {
                _logger.LogWarning("Event {Id} not found", dto.Id);
                throw new KeyNotFoundException($"Event {dto.Id} not found.");
            }

            _validation.ValidateEventDetails(
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice);

            ev.UpdateDetails(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );

            await _repo.UpdateAsync(ev);
            _logger.LogInformation("Updated Event {Id}", dto.Id);
            return Unit.Value;
        }
    }
}
