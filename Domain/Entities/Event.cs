using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Event
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime EventDate { get; private set; }
        public decimal NormalPrice { get; private set; }
        public decimal VIPPrice { get; private set; }
        public decimal BackstagePrice { get; private set; }
        public int VenueId { get; private set; }
        public Venue Venue { get; private set; } = null!;
        public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();

        private Event() { }

        public Event(
            string name,
            DateTime eventDate,
            decimal normalPrice,
            decimal vipPrice,
            decimal backstagePrice,
            int venueId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (eventDate <= DateTime.UtcNow.Date)
                throw new ArgumentException("Event date must be in the future.", nameof(eventDate));
            if (normalPrice < 0 || vipPrice < 0 || backstagePrice < 0)
                throw new ArgumentException("Prices must be non negative.");
            if (venueId <= 0)
                throw new ArgumentException("VenueId must be provided.", nameof(venueId));

            Name = name;
            EventDate = eventDate;
            NormalPrice = normalPrice;
            VIPPrice = vipPrice;
            BackstagePrice = backstagePrice;
            VenueId = venueId;
        }

        public void UpdateDetails(
            string name,
            DateTime eventDate,
            decimal normalPrice,
            decimal vipPrice,
            decimal backstagePrice,
            int venueId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (eventDate <= DateTime.UtcNow.Date)
                throw new ArgumentException("Event date must be in the future.", nameof(eventDate));
            if (normalPrice < 0 || vipPrice < 0 || backstagePrice < 0)
                throw new ArgumentException("Prices must be non negative.");
            if (venueId <= 0)
                throw new ArgumentException("VenueId must be provided.", nameof(venueId));

            Name = name;
            EventDate = eventDate;
            NormalPrice = normalPrice;
            VIPPrice = vipPrice;
            BackstagePrice = backstagePrice;
            VenueId = venueId;
        }
    }
}
