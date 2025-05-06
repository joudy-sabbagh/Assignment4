using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class DeleteTicketHandler : IRequestHandler<DeleteTicketCommand>
    {
        private readonly ITicketRepository _ticketRepo;

        public DeleteTicketHandler(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepo.GetByIdAsync(request.TicketId);
            if (ticket == null) throw new Exception("Ticket not found");

            await _ticketRepo.DeleteAsync(request.TicketId);
        }
    }
}
