// Application/UseCases/Tickets/UpdateTicketCommand.cs
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Tickets
{
    // now implements IRequest<Unit>
    public class UpdateTicketCommand : IRequest<Unit>
    {
        public UpdateTicketDTO Dto { get; }
        public UpdateTicketCommand(UpdateTicketDTO dto) => Dto = dto;
    }
}
