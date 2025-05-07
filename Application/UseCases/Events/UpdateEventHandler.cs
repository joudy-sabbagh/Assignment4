// Application/UseCases/Events/UpdateEventHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Events
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository _repo;

        public UpdateEventHandler(IEventRepository repo) => _repo = repo;

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var ev = await _repo.GetByIdAsync(dto.Id);
            if (ev == null)
                throw new KeyNotFoundException($"Event with ID {dto.Id} not found.");

            // use domain UpdateDetails method
            ev.UpdateDetails(
                dto.Name,
                dto.EventDate,
                dto.NormalPrice,
                dto.VIPPrice,
                dto.BackstagePrice,
                dto.VenueId
            );
            await _repo.UpdateAsync(ev);
        }
    }
}
