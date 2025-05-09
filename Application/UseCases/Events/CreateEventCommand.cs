using Application.DTOs;
using MediatR;

namespace Application.UseCases.Events
{
    public class CreateEventCommand : IRequest<int>
    {
        public CreateEventDTO Dto { get; }
        public CreateEventCommand(CreateEventDTO dto) => Dto = dto;
    }
}
