// Domain/Entities/Attendee.cs
using System;
using System.Collections.Generic;
using Domain.ValueObjects;  // for EmailAddress

namespace Domain.Entities
{
    public class Attendee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();

        // EF Core
        private Attendee() { }

        public Attendee(string name, EmailAddress email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            Name = name;
            Email = email;
        }

        public void Update(string name, EmailAddress email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            Name = name;
            Email = email;
        }
    }
}
