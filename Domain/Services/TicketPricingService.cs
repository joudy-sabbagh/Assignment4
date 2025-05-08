using Domain.Entities;

namespace Domain.Services
{
    public class TicketPricingService : ITicketPricingService
    {
        public decimal GetPrice(Event ev, string ticketType)
        {
            return ticketType switch
            {
                "VIP" => ev.VIPPrice,
                "Backstage" => ev.BackstagePrice,
                _ => ev.NormalPrice
            };
        }
    }
}
