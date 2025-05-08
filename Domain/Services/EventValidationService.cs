// Domain/Services/EventValidationService.cs
using System;

namespace Domain.Services
{
    public class EventValidationService : IEventValidationService
    {
        public void ValidateEventDetails(
            DateTime eventDate,
            decimal normalPrice,
            decimal vipPrice,
            decimal backstagePrice)
        {
            if (eventDate <= DateTime.UtcNow)
                throw new ArgumentException("Event date must be in the future.");

            if (normalPrice < 0 || vipPrice < 0 || backstagePrice < 0)
                throw new ArgumentException("Prices cannot be negative.");

            if (vipPrice <= normalPrice)
                throw new ArgumentException("VIP price must be greater than normal price.");

            if (backstagePrice <= vipPrice)
                throw new ArgumentException("Backstage price must be greater than VIP price.");
        }
    }
}
