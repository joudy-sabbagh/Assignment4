using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Event : IValidatableObject
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }

        public decimal NormalPrice { get; set; }
        public decimal VIPPrice { get; set; }
        public decimal BackstagePrice { get; set; }

        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventDate < DateTime.Today)
                yield return new ValidationResult("Event date must be in the future.", new[] { nameof(EventDate) });
        }
    }
}
