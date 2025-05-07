// Application/UseCases/Events/UpdateEventCommand.cs
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Events
{
    // now implements IRequest<Unit>
    public class UpdateEventCommand : IRequest<Unit>
    {
        public UpdateEventDTO Dto { get; }
        public UpdateEventCommand(UpdateEventDTO dto) => Dto = dto;
    }
}
