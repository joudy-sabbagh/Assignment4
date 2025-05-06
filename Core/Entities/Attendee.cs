using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models
{
    public class Attendee
    {
        public int AttendeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // An Attendee can have many Tickets.
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
