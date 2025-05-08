using MediatR;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Tickets
{
    public class TicketPurchasedEventHandler : INotificationHandler<TicketPurchasedEvent>
    {
        private readonly ILogger<TicketPurchasedEventHandler> _logger;
        public TicketPurchasedEventHandler(ILogger<TicketPurchasedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TicketPurchasedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ticket purchased with ID {TicketId}", notification.TicketId);
            return Task.CompletedTask;
        }
    }
}
