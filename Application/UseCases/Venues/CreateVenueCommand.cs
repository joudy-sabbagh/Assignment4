// Application/UseCases/Venues/CreateVenueCommand.cs
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Venues
{
    public class CreateVenueCommand : IRequest<int>
    {
        public CreateVenueDTO Dto { get; }
        public CreateVenueCommand(CreateVenueDTO dto) => Dto = dto;
    }
}
