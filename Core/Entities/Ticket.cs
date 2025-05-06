using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCApp.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        // The price will be set automatically based on the Event's pricing.
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ticket category is required.")]
        public TicketCategory Category { get; set; }

        [Required(ErrorMessage = "Event is required.")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Required(ErrorMessage = "Attendee is required.")]
        public int AttendeeId { get; set; }

        [ForeignKey("AttendeeId")]
        public Attendee? Attendee { get; set; }
    }
}
