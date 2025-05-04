using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "Venue name must start with a capital letter.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        // A Venue can host many Events.
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
