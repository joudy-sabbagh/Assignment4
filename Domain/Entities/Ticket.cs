using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCApp.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        // The price will be set automatically based on the Event's pricing.
        public decimal Price { get; set; }

        public TicketCategory Category { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int AttendeeId { get; set; }
        public Attendee? Attendee { get; set; }
    }
}
