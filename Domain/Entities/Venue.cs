using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Venue
    {
        public int VenueId { get; set; }
        public int Id { get => VenueId; set => VenueId = value; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;

        // A Venue can host many Events.
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
