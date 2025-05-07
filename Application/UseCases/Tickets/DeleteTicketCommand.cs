// Application/UseCases/Tickets/DeleteTicketCommand.cs
using MediatR;

namespace Application.UseCases.Tickets
{
    // now implements IRequest<Unit>
    public class DeleteTicketCommand : IRequest<Unit>
    {
        public int TicketId { get; }
        public DeleteTicketCommand(int ticketId) => TicketId = ticketId;
    }
}
