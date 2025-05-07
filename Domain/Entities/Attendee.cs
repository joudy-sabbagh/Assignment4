// Domain/Entities/Attendee.cs
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Attendee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();

        // EF Core
        private Attendee() { }

        // Creation with guards
        public Attendee(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("A valid email is required.", nameof(email));

            Name = name;
            Email = email;
        }

        // Mutation with the same guards
        public void Update(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("A valid email is required.", nameof(email));

            Name = name;
            Email = email;
        }
    }
}
