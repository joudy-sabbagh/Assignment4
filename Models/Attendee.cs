using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId { get; set; }

        [Required(ErrorMessage = "Attendee name is required.")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "Attendee name must start with a capital letter.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        // An Attendee can have many Tickets.
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
