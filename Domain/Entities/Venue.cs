using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Venue
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public string Location { get; private set; }
        public ICollection<Event> Events { get; private set; } = new List<Event>();

        private Venue() { }

        public Venue(string name, int capacity, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required.", nameof(location));

            Name = name;
            Capacity = capacity;
            Location = location;
        }

        public void Update(string name, int capacity, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required.", nameof(location));

            Name = name;
            Capacity = capacity;
            Location = location;
        }
    }
}
