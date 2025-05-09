using System;

namespace Domain.Services
{
    public interface IEventValidationService
    {
        void ValidateEventDetails(
            DateTime eventDate,
            decimal normalPrice,
            decimal vipPrice,
            decimal backstagePrice);
    }
}
