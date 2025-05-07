// Domain/Entities/Ticket.cs
using System;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; private set; }
        public string TicketType { get; private set; }
        public decimal Price { get; private set; }
        public TicketCategory Category { get; private set; }
        public int EventId { get; private set; }
        public Event Event { get; private set; } = null!;
        public int AttendeeId { get; private set; }
        public Attendee Attendee { get; private set; } = null!;

        // EF Core
        private Ticket() { }

        // Creation with guards
        public Ticket(
            string ticketType,
            decimal price,
            TicketCategory category,
            int eventId,
            int attendeeId)
        {
            if (string.IsNullOrWhiteSpace(ticketType))
                throw new ArgumentException("Ticket type is required.", nameof(ticketType));
            if (price < 0)
                throw new ArgumentException("Price must be non negative.", nameof(price));
            if (eventId <= 0)
                throw new ArgumentException("EventId must be provided.", nameof(eventId));
            if (attendeeId <= 0)
                throw new ArgumentException("AttendeeId must be provided.", nameof(attendeeId));

            TicketType = ticketType;
            Price = price;
            Category = category;
            EventId = eventId;
            AttendeeId = attendeeId;
        }

        // Mutation helper for updates
        public void UpdateTypeAndPrice(
            string ticketType,
            decimal price,
            TicketCategory category,
            int eventId,
            int attendeeId)
        {
            if (string.IsNullOrWhiteSpace(ticketType))
                throw new ArgumentException("Ticket type is required.", nameof(ticketType));
            if (price < 0)
                throw new ArgumentException("Price must be non negative.", nameof(price));
            if (eventId <= 0)
                throw new ArgumentException("EventId must be provided.", nameof(eventId));
            if (attendeeId <= 0)
                throw new ArgumentException("AttendeeId must be provided.", nameof(attendeeId));

            TicketType = ticketType;
            Price = price;
            Category = category;
            EventId = eventId;
            AttendeeId = attendeeId;
        }
    }
}
