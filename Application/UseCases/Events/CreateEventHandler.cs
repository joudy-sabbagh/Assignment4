// Application/UseCases/Events/CreateEventHandler.cs
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Events
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IEventRepository _eventRepo;

        public CreateEventHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Instantiate via your domain constructor (with guard clauses)
            var newEvent = new Event(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );

            await _eventRepo.AddAsync(newEvent);

            // Return the new single Id
            return newEvent.Id;
        }
    }
}
