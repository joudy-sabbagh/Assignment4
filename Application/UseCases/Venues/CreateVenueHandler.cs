// Application/UseCases/Venues/CreateVenueHandler.cs
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venues
{
    public class CreateVenueHandler : IRequestHandler<CreateVenueCommand, int>
    {
        private readonly IVenueRepository _venueRepo;
        private readonly ILogger<CreateVenueHandler> _logger;

        public CreateVenueHandler(IVenueRepository venueRepo, ILogger<CreateVenueHandler> logger)
        {
            _venueRepo = venueRepo;
            _logger = logger;
        }

        public async Task<int> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating venue {@Dto}", request.Dto);

            var dto = request.Dto;
            var venue = new Venue(dto.Name, dto.Capacity, dto.Location);

            await _venueRepo.AddAsync(venue);
            _logger.LogInformation("Created Venue with Id {VenueId}", venue.Id);
            return venue.Id;
        }
    }
}
