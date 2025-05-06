using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class CreateVenueHandler : IRequestHandler<CreateVenueCommand, int>
    {
        private readonly IVenueRepository _venueRepo;

        public CreateVenueHandler(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public async Task<int> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var venue = new Venue
            {
                Name = dto.Name,
                Capacity = dto.Capacity
            };

            await _venueRepo.AddAsync(venue);
            return venue.Id;
        }
    }
}
