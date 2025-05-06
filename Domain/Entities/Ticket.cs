using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int Id { get => TicketId; set => TicketId = value; }
        public string TicketType { get; set; } = string.Empty;

        // The price will be set automatically based on the Event's pricing.
        public decimal Price { get; set; }

        public TicketCategory Category { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int AttendeeId { get; set; }
        public Attendee? Attendee { get; set; }
    }
}
