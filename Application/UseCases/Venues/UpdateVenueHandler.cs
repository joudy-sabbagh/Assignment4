using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class UpdateVenueHandler : IRequestHandler<UpdateVenueCommand>
    {
        private readonly IVenueRepository _venueRepo;

        public UpdateVenueHandler(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public async Task Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var venue = await _venueRepo.GetByIdAsync(dto.VenueId);
            if (venue == null) throw new Exception("Venue not found");

            venue.Name = dto.Name;
            venue.Capacity = dto.Capacity;

            await _venueRepo.UpdateAsync(venue);
        }
    }
}
