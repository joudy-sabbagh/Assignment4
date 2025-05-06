using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Events
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository _eventRepo;

        public UpdateEventHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (ev == null) throw new Exception("Event not found");

            ev.Name = dto.Name;
            ev.EventDate = dto.Date;
            ev.NormalPrice = dto.NormalPrice;
            ev.VIPPrice = dto.VipPrice;
            ev.BackstagePrice = dto.BackstagePrice;
            ev.VenueId = dto.VenueId;

            await _eventRepo.UpdateAsync(ev);
        }
    }
}
