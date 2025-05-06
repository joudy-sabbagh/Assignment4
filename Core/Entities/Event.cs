using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCApp.Models
{
    public class Event : IValidatableObject
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "Event name must start with a capital letter.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Normal price is required.")]
        public decimal NormalPrice { get; set; }

        [Required(ErrorMessage = "VIP price is required.")]
        public decimal VIPPrice { get; set; }

        [Required(ErrorMessage = "Backstage price is required.")]
        public decimal BackstagePrice { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventDate < DateTime.Today)
            {
                yield return new ValidationResult("Event date must be in the future.", new[] { nameof(EventDate) });
            }
        }
    }
}
