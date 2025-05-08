using MediatR;

namespace Domain.Events
{
    public class TicketPurchasedEvent : INotification
    {
        public int TicketId { get; }
        public TicketPurchasedEvent(int ticketId)
        {
            TicketId = ticketId;
        }
    }
}
