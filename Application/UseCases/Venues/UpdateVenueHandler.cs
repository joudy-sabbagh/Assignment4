using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class UpdateVenueHandler : IRequestHandler<UpdateVenueCommand, bool>
    {
        private readonly IVenueRepository _venueRepo;

        public UpdateVenueHandler(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public async Task<bool> Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var venue = await _venueRepo.GetByIdAsync(dto.VenueId);
            if (venue == null) return false;

            venue.Name = dto.Name;
            venue.Capacity = dto.Capacity;

            await _venueRepo.UpdateAsync(venue); 
            return true;
        }
    }

}
