// Application/UseCases/Venues/UpdateVenueHandler.cs
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class UpdateVenueHandler : IRequestHandler<UpdateVenueCommand, bool>
    {
        private readonly IVenueRepository _repo;

        public UpdateVenueHandler(IVenueRepository repo) => _repo = repo;

        public async Task<bool> Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var venue = await _repo.GetByIdAsync(dto.Id);
            if (venue == null) return false;

            // use domain Update method
            venue.Update(dto.Name, dto.Capacity, dto.Location);
            await _repo.UpdateAsync(venue);
            return true;
        }
    }
}
