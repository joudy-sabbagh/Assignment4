// Domain/Entities/Ticket.cs
using System;
using Domain.ValueObjects;  // for Money

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; private set; }
        public string TicketType { get; private set; }
        public Money Price { get; private set; }
        public TicketCategory Category { get; private set; }
        public int EventId { get; private set; }
        public Event Event { get; private set; } = null!;
        public int AttendeeId { get; private set; }
        public Attendee Attendee { get; private set; } = null!;

        // EF Core
        private Ticket() { }

        public Ticket(
            string ticketType,
            Money price,
            TicketCategory category,
            int eventId,
            int attendeeId)
        {
            if (string.IsNullOrWhiteSpace(ticketType))
                throw new ArgumentException("Ticket type is required.", nameof(ticketType));
            TicketType = ticketType;

            Price = price;

            Category = category;
            EventId = eventId;
            AttendeeId = attendeeId;
        }

        public void UpdateTypeAndPrice(
            string ticketType,
            Money price,
            TicketCategory category,
            int eventId,
            int attendeeId)
        {
            if (string.IsNullOrWhiteSpace(ticketType))
                throw new ArgumentException("Ticket type is required.", nameof(ticketType));
            TicketType = ticketType;

            Price = price;

            Category = category;
            EventId = eventId;
            AttendeeId = attendeeId;
        }
    }
}
