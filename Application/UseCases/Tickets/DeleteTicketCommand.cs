using MediatR;

namespace Application.UseCases.Tickets
{
    public class DeleteTicketCommand : IRequest<Unit>
    {
        public int TicketId { get; }
        public DeleteTicketCommand(int ticketId) => TicketId = ticketId;
    }
}
