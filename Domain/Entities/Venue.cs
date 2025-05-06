using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // A Venue can host many Events.
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
