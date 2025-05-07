// Application/UseCases/Venues/CreateVenueHandler.cs
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class CreateVenueHandler : IRequestHandler<CreateVenueCommand, int>
    {
        private readonly IVenueRepository _venueRepo;

        public CreateVenueHandler(IVenueRepository venueRepo) =>
            _venueRepo = venueRepo;

        public async Task<int> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Use domain constructor (with guard clauses)
            var venue = new Venue(
                dto.Name,
                dto.Capacity,
                dto.Location
            );

            await _venueRepo.AddAsync(venue);
            return venue.Id;
        }
    }
}
