using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Attendee
    {
        public int AttendeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // alias so handlers/controllers can use .Id
        public int Id
        {
            get => AttendeeId;
            set => AttendeeId = value;
        }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
