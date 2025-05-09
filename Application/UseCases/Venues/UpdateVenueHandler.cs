using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venues
{
    public class UpdateVenueHandler : IRequestHandler<UpdateVenueCommand, Unit>
    {
        private readonly IVenueRepository _repo;
        private readonly ILogger<UpdateVenueHandler> _logger;

        public UpdateVenueHandler(IVenueRepository repo, ILogger<UpdateVenueHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            _logger.LogInformation("Updating venue {Id} with {@Dto}", dto.Id, dto);

            var venue = await _repo.GetByIdAsync(dto.Id);
            if (venue == null)
            {
                _logger.LogWarning("Venue {Id} not found", dto.Id);
                throw new KeyNotFoundException($"Venue with ID {dto.Id} not found.");
            }

            venue.Update(dto.Name, dto.Capacity, dto.Location);
            await _repo.UpdateAsync(venue);

            _logger.LogInformation("Updated venue {Id}", dto.Id);
            return Unit.Value;
        }
    }
}
