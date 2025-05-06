using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

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
            var newEvent = new Event
            {
                Name = dto.Name,
                EventDate = dto.Date,
                NormalPrice = dto.NormalPrice,
                VIPPrice = dto.VipPrice,
                BackstagePrice = dto.BackstagePrice,
                VenueId = dto.VenueId
            };

            await _eventRepo.AddAsync(newEvent);
            return newEvent.Id;
        }
    }
}
