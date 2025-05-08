// Domain/Services/IEventValidationService.cs
using System;

namespace Domain.Services
{
    /// <summary>
    /// Encapsulates all business rules for creating/updating Events.
    /// </summary>
    public interface IEventValidationService
    {
        void ValidateEventDetails(
            DateTime eventDate,
            decimal normalPrice,
            decimal vipPrice,
            decimal backstagePrice);
    }
}
