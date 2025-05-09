using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class DeleteTicketHandler : IRequestHandler<DeleteTicketCommand, Unit>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly ILogger<DeleteTicketHandler> _logger;

        public DeleteTicketHandler(
            ITicketRepository ticketRepo,
            ILogger<DeleteTicketHandler> logger)
        {
            _ticketRepo = ticketRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting ticket {TicketId}", request.TicketId);

            var ticket = await _ticketRepo.GetByIdAsync(request.TicketId)
                         ?? throw new KeyNotFoundException($"Ticket {request.TicketId} not found");

            await _ticketRepo.DeleteAsync(request.TicketId);

            _logger.LogInformation("Deleted ticket {TicketId}", request.TicketId);
            return Unit.Value;
        }
    }
}
