using Application.DTOs;
using MediatR;

namespace Application.UseCases.Events
{
    public class UpdateEventCommand : IRequest<Unit>
    {
        public UpdateEventDTO Dto { get; }
        public UpdateEventCommand(UpdateEventDTO dto) => Dto = dto;
    }
}
