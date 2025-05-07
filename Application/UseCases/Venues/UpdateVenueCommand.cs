// Application/UseCases/Venues/UpdateVenueCommand.cs
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Venues
{
    // Now implements IRequest<Unit>
    public class UpdateVenueCommand : IRequest<Unit>
    {
        public UpdateVenueDTO Dto { get; }
        public UpdateVenueCommand(UpdateVenueDTO dto) => Dto = dto;
    }
}
