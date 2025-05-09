using Application.DTOs;
using MediatR;

namespace Application.UseCases.Venues
{
    public class UpdateVenueCommand : IRequest<Unit>
    {
        public UpdateVenueDTO Dto { get; }
        public UpdateVenueCommand(UpdateVenueDTO dto) => Dto = dto;
    }
}
